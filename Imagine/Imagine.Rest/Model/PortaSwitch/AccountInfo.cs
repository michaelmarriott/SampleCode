using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Imagine.Rest.Helper.Authentication;
using Imagine.Rest.Model;

namespace Imagine.Rest.PortaSwitch.Account {

  #region Enumerations

  /// <summary>
  /// N - No forwarding
  /// Y - Follow-me
  /// F - Advanced forwarding
  /// U - Forward to SIP URI
  /// C - Forward to CLD
  /// </summary>
  public enum CallForwardingOptions {
    NoForwarding = 0,
    FollowMe = 1,
    AdvancedForwarding = 2,
    ForwardToSIP = 3,
    ForwardToCLD = 4
  }

  /// <summary>
  /// -1 - Debit
  /// 0 - RechargeVoucher
  /// 1 - Credit
  /// 2 - Alias
  /// </summary>
  public enum BillingModel {
    Debit = -1,
    RechargeVoucher = 0,
    Credit = 1,
    Alias = 2
  }

  #endregion Enumerations

  public partial class AccountInfo : WebServiceModel<AccountInfo> {

    #region Private Variables

    private static ILog log = LogManager.GetLogger(typeof(AccountInfo));
    private DateTime? CancellationDate;
    private AuthInfoStructure authInfo;

    #endregion Private Variables

    #region Constructor

    /// <summary> Default Constructor to create an AccountInfo model </summary>
    public AccountInfo() {
      CancellationDate = null;
      CreateAuthInfo();
    }

    #endregion Constructor

    #region Public Methods

    /// <summary>
    /// Loads the Aliases associated with the account
    /// </summary>
    public List<String> LoadAliases() {
      List<String> aliases = new List<string>();
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.get_alias_list(new GetAccountAliasListRequest() { i_master_account = i_account });
        foreach (AliasInfo alias in response.alias_list) {
          aliases.Add(alias.id);
        }
      }
      return aliases;
    }

    public AccountInfo Find(int id) {
      var infoRequest = new GetAccountInfoRequest() { i_account = id };
      var accountResponse = GetAccountInfo(infoRequest);
      return accountResponse;
    }

    /// <summary>
    /// Finds the Account for the given identifier
    /// </summary>
    /// <param name="id">Account Identifier</param>
    /// <returns>The Account for the identifier</returns>
    public AccountInfo Find(string id) {
      var infoRequest = new GetAccountInfoRequest() { id = id };
      var accountResponse = GetAccountInfo(infoRequest);
      return accountResponse;
    }

    /// <summary>
    /// Find all of the accounts using the customer identifier as a search filter
    /// </summary>
    /// <param name="id">Customer Identifier to filter accounts by</param>
    /// <param name="limit">Number of customers to retrieve (Default = 10)</param>
    /// <param name="offset">Offset from 0 for the customers (Default = 0)</param>
    /// <returns>A Array of accounts for the given customer</returns>
    public AccountInfo[] FindByCustomerId(int id, int limit = 10, int offset = 0) {
      AccountInfo[] accounts = new AccountInfo[0];
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.get_account_list(new GetAccountListRequest() { get_not_closed_accounts = 1, get_not_closed_accountsSpecified = true, i_customer = id, limit = limit, offset = offset });
        if (response.account_list != null) {
          accounts = response.account_list;
        }
      }
      return accounts;
    }

    /// <summary>
    /// Saves the Current Account Information into the WebService
    /// </summary>
    /// <returns>True if the account was saved, otherwise false</returns>
    public bool Save() {
      bool updated = false;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.update_account(new UpdateAccountRequest() { account_info = this });
        updated = response.i_account > 0;
      }
      if (updated) {
        ConfigureServiceFeatures();
        UpdateCustomFields();
      }
      return updated;
    }

    public bool Create() {
      bool created = false;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.add_account(new AddAccountRequest() { account_info = this });
        created = response.i_account > 0;
        this.i_account = response.i_account;
      }
      if (created) {
        ConfigureServiceFeatures();
        UpdateCustomFields();
      }
      return created;
    }

    public bool Delete() {
      bool deleted = false;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.terminate_account(new TerminateAccountRequest() { i_account = this.i_account });
        deleted = response.success > 0;
      }
      return deleted;
    }

    public bool Move() {
      bool moved = false;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var response = service.move_account(new MoveAccountRequest() { i_account = this.i_account, i_customer = this.i_customer });
        moved = response.old_i_account == this.i_account;
      }
      return moved;
    }

    public FollowMeNumberInfo[] LoadFollowMeNumbers() {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      FollowMeNumberInfo[] followMeNumbers = null;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        GetAccountFollowMeResponse followMeResponse = service.get_account_followme(new GetAccountFollowMeRequest() { i_account = i_account });
        if (followMeResponse.followme_numbers != null) {
          followMeNumbers = followMeResponse.followme_numbers;
        }
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "get_account_followme", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return followMeNumbers;
    }

    /// <summary>
    /// must update_account_followme before calling add_followme_number, to instatiate a link between the accoutn and follow me.
    /// </summary>
    public int AddFollowMeNumber(FollowMeNumberInfo number) {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      int result = 0;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var updateFollowMe = service.update_account_followme(new UpdateAccountFollowMeRequest() {
          i_account = number.i_account,
          followme_info = new FollowMeInfo() {
            i_account = number.i_account,
            i_accountSpecified = number.i_account > 0,
            sequence = "Order"
          }
        });
        AddUpdateFollowMeNumberResponse followMeResponse = service.add_followme_number(new AddFollowMeNumberRequest() { number_info = number });
        result = followMeResponse.i_follow_me_number;
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "add_followme_number", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return result;
    }

    public bool DeleteFollowMeNumber(int id) {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      bool result = false;
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        var followMeResponse = service.delete_followme_number(new DeleteFollowMeNumberRequest() { i_follow_me_number = id });
        result = followMeResponse.success > 0;
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "delete_followme_number", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return result;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary> Build the Account Entity from the Account Info </summary>
    /// <param name="infoRequest">Account info from the webservice</param>
    /// <returns>A new account entity object</returns>
    private AccountInfo GetAccountInfo(GetAccountInfoRequest infoRequest) {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      AccountInfo info = null;
      try {
        using (var service = new AccountAdminService()) {
          service.AuthInfoStructureValue = authInfo;
          var response = service.get_account_info(infoRequest);
          if (response != null) {
            info = response.account_info;
          }
        }
      } catch (Exception ex) {
        throw ex;
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "get_account_info", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return info;
    }

    private void CreateAuthInfo() {
      var authentication = new ServiceAuthentication();
      authInfo = new AuthInfoStructure() {
        login = authentication.UserName,
        password = authentication.Password
      };
    }

    /// <summary>
    /// Configure the service features on the account
    /// </summary>
    private void ConfigureServiceFeatures() {
      log.Debug("ConfigureServiceFeatures");
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      try {
        using (var service = new AccountAdminService()) {
          service.AuthInfoStructureValue = authInfo;
          var result = service.get_service_features(new GetAccountServiceFeaturesRequest() { i_account = i_account });
          ServiceFeatureInfo[] info = new ServiceFeatureInfo[1];
          if (result.service_features.Length > 0) {
            result.service_features[0].attributes[0].values[0] = id;
            result.service_features[0].attributes[1].values[0] = "A";
            info[0] = result.service_features[0];
          }
          var updateResult = service.update_service_features(new UpdateAccountServiceFeaturesRequest() { i_account = i_account, service_features = info });
        }
      } catch (Exception ex) {
        log.Error(string.Format("Failure when updating the service features. Message {0}, StackTrace {1}", ex.Message, ex.StackTrace), ex);
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "update_service_features", execution_time_ms = benchmark.ElapsedMilliseconds }));
    }

    /// <summary> Updates the custom fields on the account for the given entity </summary>
    private void UpdateCustomFields() {
      log.Debug("UpdateCustomFields");
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      using (var service = new AccountAdminService()) {
        service.AuthInfoStructureValue = authInfo;
        // Get the existing custom values
        var customFields = service.get_custom_fields_values(new GetAccountCustomFieldsValuesRequest() { i_account = i_account });
        if (customFields != null && customFields.custom_fields_values != null) {
          // This is a really crap way to set data, but the custom fields are not important at the moment
          for (int i = 0; i < customFields.custom_fields_values.Length; i++) {
            switch (customFields.custom_fields_values[i].name) {
              case "TrunkID":
                customFields.custom_fields_values[i].db_value = id;
                customFields.custom_fields_values[i].text_value = id;
                break;

              case "Region":
                customFields.custom_fields_values[i].db_value = "JHB";
                customFields.custom_fields_values[i].text_value = "JHB";
                break;

              case "IP Info":
                customFields.custom_fields_values[i].db_value = "0.0.0.0";
                customFields.custom_fields_values[i].text_value = "0.0.0.0";
                break;

              case "Device Type":
                customFields.custom_fields_values[i].db_value = "SIP Account";
                customFields.custom_fields_values[i].text_value = "SIP Account";
                break;

              case "Cancellation Date":
                customFields.custom_fields_values[i].db_value = CancellationDate.HasValue ? ((DateTime)CancellationDate).ToString("yyyy-MM-dd hh:mm:ss") : string.Empty;
                customFields.custom_fields_values[i].text_value = CancellationDate.HasValue ? ((DateTime)CancellationDate).ToString("yyyy-MM-dd hh:mm:ss") : string.Empty;
                break;
            }
          }
          log.Debug(JsonConvert.SerializeObject(new UpdateAccountCustomFieldsValuesRequest() { i_account = i_account, custom_fields_values = customFields.custom_fields_values }));
          // Add the custom values
          var updateFields = service.update_custom_fields_values(new UpdateAccountCustomFieldsValuesRequest() {
            i_account = i_account,
            custom_fields_values = customFields.custom_fields_values
          });
        }
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "update_custom_fields_values", execution_time_ms = benchmark.ElapsedMilliseconds }));
    }

    #endregion Private Methods
  }
}