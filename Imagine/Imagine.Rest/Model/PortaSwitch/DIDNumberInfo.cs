using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using Imagine.Rest.Data;
using Imagine.Rest.Helper.Authentication;
using Imagine.Rest.Model;
using Imagine.Rest.PortaSwitch.Account;

namespace Imagine.Rest.PortaSwitch.DID {
  public partial class DIDNumberInfo : WebServiceModel<DIDNumberInfo> {

    public DIDNumberInfo() {
      CreateAuthInfo();
    }


    #region Private Variables
    /// <summary> Amount of time to reserve the did number for. Should this be a vallue in the config files? </summary>
    private const int RESERVETIME = 864000;
    private static readonly ILog log = LogManager.GetLogger(typeof(DIDNumberInfo));
    private Imagine.Rest.PortaSwitch.DID.AuthInfoStructure authInfo;
    #endregion

    public DIDNumberInfo Find(int id) { throw new NotImplementedException(); }

    public DIDNumberInfo Find(string id) { throw new NotImplementedException(); }

    public bool Create() { throw new NotImplementedException(); }

    public bool Save() { throw new NotImplementedException(); }

    public bool Delete() { throw new NotImplementedException(); }

    /// <summary>
    /// Only retrieve numbers which are older than 90 days since being made available
    /// Validate the result set and reserves for 3 months to prevent the next request from retrieving the same numbers
    /// Have the right number of numbers, now attempt to reserve the numbers
    /// </summary>
    /// <param name="resellerIdentifier"></param>
    /// <param name="amount"></param>
    /// <param name="blockRequest"></param>
    /// <returns></returns>
    public List<String> Reserve(string resellerIdentifier, int amount, bool blockRequest, string prefix) {
      var config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      int enviroment = int.Parse(config.Environment);
      var reservedNumbers = new List<String>();
      try {
        DateTime reservedExpire = DateTime.Now.Subtract(new TimeSpan(0, 0, RESERVETIME));
        DateTime releaseExpire = DateTime.Now.Subtract(new TimeSpan(90, 0, 0, 0));
        using (Imagine.Rest.Data.Entities context = new Imagine.Rest.Data.Entities()) {
          var vendorBatch = GetVendorBatch(enviroment, resellerIdentifier, context);
          var didList = GetDidList(enviroment, releaseExpire, context, vendorBatch, prefix);
          for (int i = 0; i < didList.Count() && reservedNumbers.Count != amount; i++) {
            DIDNUMBERINVENTORY did = didList.ElementAt<DIDNUMBERINVENTORY>(i);
            bool assignedNumber = false;
            AccountInfo account = new AccountInfo().Find((string)did.PHONENUMBER);
            if (account != null && account.bill_status == "O") {
              assignedNumber = true;
            }
            if (!assignedNumber) {
              ReserveDIDNumberResponse reserve = ReserveDIDNumber(did);
              if (reserve != null && reserve.success == 1) {
                if (!reservedNumbers.Contains(did.PHONENUMBER)) {
                  reservedNumbers.Add(did.PHONENUMBER);
                }
              }
            }
          }
        }
      } catch (Exception e) {
        if (e.InnerException != null)
          throw e.InnerException;
        throw;
      } finally {
        if (reservedNumbers.Count != amount) {
          foreach (var number in reservedNumbers) {
            UnReserveDIDNumber(number);
          }
        }
      }
      return reservedNumbers;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="resellerId"></param>
    /// <param name="amount"></param>
    /// <param name="isBlockSet"></param>
    /// <param name="prefix"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public List<String> Find(string resellerId, int amount, bool isBlockSet, string prefix, int limit, int offset) {
      var config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      int enviroment = int.Parse(config.Environment);
      var availableNumbers = new List<String>();
      try {
        DateTime reservedExpire = DateTime.Now.Subtract(new TimeSpan(0, 0, RESERVETIME));
        DateTime releaseExpire = DateTime.Now.Subtract(new TimeSpan(90, 0, 0, 0));
        using (Imagine.Rest.Data.Entities context = new Imagine.Rest.Data.Entities()) {
          var vendorBatch = GetVendorBatch(enviroment, resellerId, context);
          var didList = GetDidList(enviroment, releaseExpire, context, vendorBatch, prefix);
          for (int i = 0; i < didList.Count() && availableNumbers.Count != amount; i++) {
            DIDNUMBERINVENTORY did = didList.ElementAt<DIDNUMBERINVENTORY>(i);
            AccountInfo account = new AccountInfo().Find((string)did.PHONENUMBER);
            if (account != null && account.bill_status == "O") {
            }else{
              availableNumbers.Add(did.PHONENUMBER);
            }
          }
        }
      } catch (Exception e) {
        if (e.InnerException != null)
          throw e.InnerException;
        throw;
      }
      return availableNumbers;
    }


    #region Private methods

    private List<DIDNUMBERINVENTORY> GetDidList(int enviroment, DateTime releaseExpire, Imagine.Rest.Data.Entities context, VENDORDIDBATCH vendorBatch, string prefix) {
      if (prefix == null) { prefix = String.Empty; }
      var didList = (from e in context.DIDNUMBERINVENTORYS
                     where e.I_DV_BATCH == vendorBatch.I_DV_BATCH && 
                     e.ENVIROMENT == enviroment &&
                     (e.RELEASED == null || e.RELEASED <= releaseExpire) && 
                     (e.I_ACCOUNT == null || e.ACCOUNT_ENVIROMENT != enviroment) && 
                     (e.RESERVED == null) &&
                     (e.PHONENUMBER.StartsWith(prefix))
                     orderby e.PHONENUMBER
                     select e).ToList();
      return didList;
    }

    private VENDORDIDBATCH GetVendorBatch(int enviroment, string resellerIdentifier, Imagine.Rest.Data.Entities context) {
      var vendorBatch = (from e in context.VENDORDIDBATCHES
                         where e.NAME == resellerIdentifier && e.I_ENV == enviroment
                         select e).First();
      return vendorBatch;
    }

    /// <summary> Reserves the DID Number in the PortaOne System</summary>
    /// <param name="did">A DID Entity Framework entity which to reserve</param>
    /// <returns>The response which indicates if the number could be reserved or not</returns>
    private ReserveDIDNumberResponse ReserveDIDNumber(DIDNUMBERINVENTORY did) {
      ReserveDIDNumberResponse reserve = null;
      using (var service = new DIDAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        reserve = service.reserve_number(new ReserveDIDNumberRequest() {
          number = did.PHONENUMBER,
          reserve_term = RESERVETIME
        });
      }
      return reserve;
    }

    private void UnReserveDIDNumber(String didNumber) {
      using (var service = new DIDAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var reserve = service.reserve_number(new ReserveDIDNumberRequest() {
          number = didNumber,
          reserve_term = 1
        });
      }
    }



    private Imagine.Rest.PortaSwitch.DID.AuthInfoStructure CreateAuthInfo() {
      var authentication = new ServiceAuthentication();
      authInfo = new Imagine.Rest.PortaSwitch.DID.AuthInfoStructure() {
        login = authentication.UserName,
        password = authentication.Password
      };
      return authInfo;
    }

    #endregion

  }
}