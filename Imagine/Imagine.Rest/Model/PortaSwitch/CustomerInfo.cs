using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Imagine.Rest.Helper.Authentication;
using Imagine.Rest.Model;
using Imagine.Rest.PortaSwitch.Account;

namespace Imagine.Rest.PortaSwitch.Customer {

  #region Enums

  /// <summary>
  /// Enumeration for the different states that a customer can be found in
  /// </summary>
  public enum CustomerStatus {
    Open = 0,
    Suspended = 1,
    Closed = 2
  }

  public enum BillingPeriod {
    Monthly = 4,
    MonthlyAnniversary = 5,
  }

  public enum CustomerType {
     Retail = 1,
     Reseller = 2,
     Distributor = 3
  }

  #endregion Enums

  /// <summary>
  /// Customer Web Service Model
  /// </summary>
  public partial class CustomerInfo : WebServiceModel<CustomerInfo> {
    #region Private Variables
    private static readonly ILog log = LogManager.GetLogger(typeof(CustomerInfo));
    private AuthInfoStructure authInfo;
    #endregion

    #region Public Methods

    public CustomerInfo() {
      CreateAuthInfo();
    }


    /// <summary>
    /// Returns a list of all of the Voice Accounts located on the current customer
    /// </summary>
    public AccountInfo[] Accounts {
      get { return new AccountInfo().FindByCustomerId(i_customer); }
    }

    /// <summary>
    /// Returns a list of all of the Voice Accounts located on the current customer
    /// </summary>
    public AccountInfo[] GetAccounts(int limit, int offset) {
      return new AccountInfo().FindByCustomerId(i_customer, limit, offset); 
    }

    public CustomerInfo Find(int id) {
      return GetCustomerInfo(new GetCustomerInfoRequest() { i_customer = id, i_customerSpecified = true });
    }

    /// <summary>
    /// Finds a customer by searching for a customer via the name
    /// </summary>
    /// <param name="id">Name of the customer to find</param>
    /// <returns>A customer object if found, otherwise null</returns>
    public CustomerInfo Find(string id) {
      return GetCustomerInfo(new GetCustomerInfoRequest() { name = id });
    }

    /// <summary>
    /// Retrieves all of the Customers for the given parameters
    /// </summary>
    /// <param name="parentId">Name of the reseller to filter the customers under</param>
    /// <param name="customerName">CustomerName to be searched</param>
    /// <param name="limit">Number of customers to retrieve</param>
    /// <param name="offset">Offset from 0 for the customers</param>
    /// <returns>An Array of customers if found, otherwise it will return an empty array</returns>
    public CustomerInfo[] FindAll(int parentId, string customerName,  int limit, int offset) {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      CustomerInfo[] customers = new CustomerInfo[0];
      using (var service = new CustomerAdminService()) {
        service.AuthInfoStructureValue = CreateAuthInfo();
        bool parentSpecified = parentId > 0 ? true : false;
        var result = service.get_customer_list(new GetCustomerListRequest() { i_parent = parentId, i_parentSpecified = parentSpecified, name = customerName, limit = limit, offset = offset });
        if (result != null && result.customer_list != null) {
          customers = result.customer_list;
        }
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "get_customer_list", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return customers;
    }

    public bool Save() {
      bool saved = false;
      try {
        using (var service = new CustomerAdminService()) {
          service.AuthInfoStructureValue = authInfo;
          CreditLimitResetFix(service);
          var response = service.update_customer(new UpdateCustomerRequest() { customer_info = this });
          saved = response.i_customer > 0;
        }
      } catch (Exception ex) {
        throw ex;
      }
      return saved;
    }

    // This fixes credit limits and credit limit extensions, which get stuck if not set to null [this is a Porta defect]
    private void CreditLimitResetFix(CustomerAdminService service) {
      var resetCustomer = (CustomerInfo)this.MemberwiseClone();
      resetCustomer.credit_limit = null;
      resetCustomer.credit_limitSpecified = true;
      resetCustomer.perm_credit_limitSpecified = false;
      resetCustomer.temp_credit_limitSpecified = false;
      var resetResponse = service.update_customer(new UpdateCustomerRequest() { customer_info = resetCustomer });
    }

    public bool Create() {
      bool saved = false;
      try {
        using (var service = new CustomerAdminService()) {
          service.AuthInfoStructureValue = authInfo;
          var response = service.add_customer(new AddCustomerRequest() { customer_info = this });
          saved = response.i_customer > 0;
        }
      } catch (Exception ex) {
        throw ex;
      }
      return saved;
    }

    public bool Delete() { throw new NotImplementedException(); }

    #endregion Public Methods

    #region Private Methods

    public AuthInfoStructure CreateAuthInfo() {
      var authentication = new ServiceAuthentication();
      authInfo = new AuthInfoStructure() {
        login = authentication.UserName,
        password = authentication.Password
      };
      return authInfo;
    }

    /// <summary>
    /// Gets the Customer Info for the given request
    /// </summary>
    /// <param name="request">Request that will be provided for the call</param>
    /// <returns>A CustomerInfo if one was found, otherwise it will return null</returns>
    private CustomerInfo GetCustomerInfo(GetCustomerInfoRequest request) {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      CustomerInfo customer = null;
      using (var service = new CustomerAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.get_customer_info(request);
        if (response != null && response.customer_info != null && response.customer_info.i_customer != 0) {
          customer = response.customer_info;
        }
      }      
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "get_customer_info", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return customer;
    }

    #endregion Private Methods
  }
}