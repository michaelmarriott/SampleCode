using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Imagine.Rest.Helper;
using Imagine.Rest.Model.PortaSwitch;
using Imagine.Rest.PortaSwitch.Account;
using Imagine.Rest.PortaSwitch.Customer;
using Imagine.Rest.PortaSwitch.Product;

namespace Imagine.Rest.ViewModel {

  /// <summary>
  /// Account View Model for the Account Model
  /// </summary>
  public class AccountViewModel : BaseViewModel {

    #region Properties

    /// <summary> Login for the account </summary>
    public string Login { get; set; }

    /// <summary> Password for the account </summary>
    public string Password { get; set; }

    /// <summary>Firstname of the account (This is not normally populate) </summary>
    public string FirstName { get; set; }

    /// <summary> Lastname of the account (This is not normally populate) </summary>
    public string LastName { get; set; }

    /// <summary> Email address for the Account </summary>
    public string Email { get; set; }

    /// <summary> Customer salutation for the Account </summary>
    public string Salutation { get; set; }

    /// <summary> List of Account Aliases for the account. </summary>
    public List<string> Aliases { get; private set; }

    /// <summary> Customer PortaSwitch Identifier. [ Please do not use this field, it will be replaced with the Customer Identifier from the Customer Object instead] </summary>
    /// <exception cref="System.ArgumentException">Customer ID Must always be greater than 0</exception>
    [Required]
    public String CustomerId { get; set; }

    /// <summary> Product Code of the account [  The Product Name / Code must match the Voxzal Call Plan ]</summary>
    [Required]
    public string ProductCode { get; set; }

    /// <summary> Hidden Id for the Product Code</summary>
    [JsonIgnore]
    [XmlIgnore]
    public int ProductId { get; set; }

    /// <summary> Billing models for the account [Debit,RechargeVoucher,Credit,Alias] </summary>
    public string BillingModel { get; set; }

    /// <summary> Initial balance on the account </summary>
    public double OpeningBalance { get; set; }

    /// <summary> Limit that the credit can get to before the accont is cut off. [ Please use the Customer CreditLimit instead ]</summary>
    public double CreditLimit { get; set; }

    /// <summary> Represents the default Credit Limit on the account [ The amount that will be reset to when the month changes ] </summary>
    public double DefaultCreditLimit { get; set; }

    /// <summary> Current balance on the the account </summary>
    public double Balance { get; private set; }

    /// <summary> Indicate the date the account was activated </summary>
    public DateTime ActivationDate { get; set; }

    /// <summary> Cancellation date for the account </summary>
    public DateTime? CancellationDate { get; set; }

    /// <summary> Expiration date of the account. Can be a future date. </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary> ndicates if the account is Open [ Closed accounts will not be able to make or receive calls ] </summary>
    public bool Open { get; set; }

    /// <summary> Indicates whether the account is suspended. </summary>
    public bool Suspended { get; set; }

    /// <summary> Allow Forwarding on account (ReadOnly) </summary>
    public bool ForwardOnly { get; set; }

    /// <summary> Indicates the state of the call forwarding [{Default} "N" = NoForwarding ,"Y" = FollowMe, "F" = AdvancedForwarding, "U" = ForwardToSIP, "C" = ForwardToCLD] </summary>
    public string CallForwarding { get; set; }

    /// <summary> List of Follow me / call forwarding numbers setup for the account </summary>
    public List<AccountFollowMeViewModel> FollowMeNumbers { get; set; }

    /// <summary> Indicates the routing plan that the account must be set with. Should normally be set to the reseller code (Prefix) </summary>
    public string RoutingPlan { get; set; }

    /// <summary> Routing password used to protect the account (Password is generated as 8 characters with alpha numeric characters) </summary>
    public string H323Password { get; set; }

    /// <summary> Indicates a Cellphone Number to associated with the given account (Used for PrePaid) </summary>
    public string CellphoneNumber { get; set; }

    #endregion Properties

    /// <summary> Casting Operator for Converting AccountInfo to AccountViewModels </summary>
    /// <param name="entity">AccountViewModel to Cast</param>
    /// <returns>AccountInfo Model</returns>
    public static explicit operator AccountInfo(AccountViewModel entity) {
      if (entity.ActivationDate <= new DateTime(2000,1,1)){
        entity.ActivationDate = DateTime.Now;
      }
      if (string.IsNullOrEmpty(entity.H323Password)) { entity.H323Password = RandomPasswordGenerator.CreateRandomPassword(12); }
      if (string.IsNullOrEmpty(entity.Login)) { entity.Login = Guid.NewGuid().ToString(); }
      if (string.IsNullOrEmpty(entity.Password)) { entity.Password = RandomPasswordGenerator.CreateRandomPassword(12); }
     
      var customer = new CustomerInfo().Find(entity.CustomerId);
      if (customer == null) { throw new ArgumentNullException("AccountInfo", "Must provide a valid customer."); }
      var reseller = new CustomerInfo().Find(customer.i_parent);
      if (reseller == null) { throw new ArgumentNullException("AccountInfo", "Must provide a valid customer with a reseller."); }
      var resellerPrefix = reseller.name.Split(char.Parse("-"))[0];
      int productID = entity.ProductId;
      if (productID == 0) {
        var product = new ProductInfo().Find(entity.ProductCode);
        if (product != null && product.i_product != null) {
          productID = (int)product.i_product;
        } else {
          throw new Exception(string.Format("Product '{0}' does not exist when creating the account '{1}'", entity.ProductCode, entity.Id));
        }
      }
      if (string.IsNullOrEmpty(entity.RoutingPlan)) { entity.RoutingPlan = resellerPrefix; }
      var routingPlanInfo = new RoutingPlanInfo();
      var routingPlan = routingPlanInfo.GetRoutesByName(entity.RoutingPlan);
      var accountInfo = new AccountInfo();
      if (entity == null) { throw new ArgumentNullException("AccountInfo", "Must provide a valid customer."); }

      accountInfo.i_account = entity.InternalId;
      accountInfo.i_accountSpecified = entity.InternalId > 0;
      accountInfo.id = entity.Id;
      accountInfo.firstname = entity.FirstName;
      accountInfo.lastname = entity.LastName;
      accountInfo.login = entity.Login;
      accountInfo.password = string.IsNullOrEmpty(entity.Password) ? RandomPasswordGenerator.CreateRandomPassword(12) : entity.Password;
      accountInfo.email = entity.Email;
      accountInfo.h323_password = entity.H323Password;
      accountInfo.balance = (float)entity.Balance;
      accountInfo.balanceSpecified = entity.Balance > 0;
      accountInfo.opening_balance = (float)entity.OpeningBalance;
      accountInfo.opening_balanceSpecified = entity.OpeningBalance > 0;
      accountInfo.billing_model = GetBillingModelString(entity.BillingModel);
      accountInfo.billing_modelSpecified = true;
      accountInfo.i_product = productID;
      accountInfo.i_productSpecified = true;
      accountInfo.i_customer = customer.i_customer;
      accountInfo.i_customerSpecified = true;
      accountInfo.credit_limit = (float)entity.CreditLimit;
      accountInfo.credit_limitSpecified = entity.CreditLimit > 0;
      accountInfo.blocked = entity.Suspended ? "Y" : "N";
      if (entity.ExpirationDate != null){
        accountInfo.expiration_date = (DateTime)entity.ExpirationDate;    
      }else{
        accountInfo.expiration_date = null;
      }
      accountInfo.expiration_dateSpecified = true;
      accountInfo.redirect_number = CreateLocation(entity.Id);
      accountInfo.phone1 = entity.CellphoneNumber;
      accountInfo.i_routing_plan = routingPlan != null ? routingPlan.Id : 0;
      accountInfo.i_routing_planSpecified = routingPlan != null ? routingPlan.Id >= 0 : false;
      accountInfo.i_time_zone = 367;
      accountInfo.i_time_zoneSpecified = true;
      accountInfo.i_lang = "en";
      accountInfo.out_date_format = "YYYY-MM-DD";
      accountInfo.out_time_format = "HH24:MI:SS";
      accountInfo.out_date_time_format = "YYYY-MM-DD HH24:MI:SS";
      accountInfo.in_date_format = "YYYY-MM-DD";
      accountInfo.in_time_format = "HH24:MI:SS";
      accountInfo.ecommerce_enabled = "N";
      accountInfo.um_enabled = "Y";
      accountInfo.activation_date = entity.ActivationDate;
      accountInfo.activation_dateSpecified = true;
      GetCallFowardingString(accountInfo, GetCallFowardingOption(entity.CallForwarding));
      return accountInfo;
    }

    /// <summary> Copy Operator to convert a AccountInfo Model to an AccountViewModel Representor </summary>
    /// <param name="info">AccountInfo Model retrieved from the WebService</param>
    /// <returns>A AccountViewModel representing the AccountInfo Model</returns>
    public static explicit operator AccountViewModel(AccountInfo info) {
      var product = new ProductInfo().Find(info.i_product);
      var customer = new CustomerInfo().Find(info.i_customer);
      var routingPlan = new RoutingPlanInfo().GetRoutesById(info.i_routing_plan);
      var entity = new AccountViewModel() {
        InternalId = info.i_account,
        Id = info.id,
        Balance = info.balance,
        ActivationDate = info.activation_date,
        Login = string.IsNullOrEmpty(info.login) ? null : info.login,
        Password = string.IsNullOrEmpty(info.password) ? RandomPasswordGenerator.CreateRandomPassword(12) : info.password,
        FirstName = info.firstname,
        LastName = info.lastname,
        Email = info.email,
        CustomerId = customer.name,   
        Open = info.bill_status == "O",
        OpeningBalance = info.opening_balance,
        CreditLimit = info.credit_limit != null ? (double)info.credit_limit : 0,
        Suspended = info.blocked == "Y",
        CallForwarding = info.follow_me_enabled,
        CellphoneNumber = info.phone1,
        ForwardOnly = string.IsNullOrEmpty(info.service_flags) || info.service_flags.Length < 8 ? false : info.service_flags.Substring(8, 1).Equals("2"),
        H323Password = info.h323_password,
        ProductCode = product.name,
        RoutingPlan = routingPlan != null ? routingPlan.Name : string.Empty,
        BillingModel = GetBillingModel(info.billing_model),
      };
      if (info.expiration_date != null){
        entity.ExpirationDate = Convert.ToDateTime(info.expiration_date);
      }
      // if (extendedInformation) { LoadAliases(ref entity);LoadCustomFields(ref entity);}
      // if (GetCallFowardingOption(entity.CallForwarding) != CallForwardingOptions.NoForwarding) {
      // FollowMeNumberInfo[] followMeNumbers = info.LoadFollowMeNumbers();
      // if (followMeNumbers != null){
      //  entity.FollowMeNumbers = followMeNumbers.ToList().ConvertAll(e => (AccountFollowMeViewModel)e);
      // }
      // }
      return entity;
    }

    /// <summary> Test Model Generator for Documentation </summary>
    /// <returns>A Test Object of AccountViewModel</returns>
    public static AccountViewModel TestModel() {
      var accountViewModel = new AccountViewModel() {
        Id = "27878058094",
        Login = "JDoe",
        Password = "xxxxxx",
        FirstName = "John",
        LastName = "Doe",
        Email = "johndoe@vox.co.za",
        Salutation = "",
        CallForwarding = GetCallFowardingOption(CallForwardingOptions.FollowMe),
        CancellationDate = DateTime.Now,
        ForwardOnly = true,
        BillingModel = GetBillingModel((int)Imagine.Rest.PortaSwitch.Account.BillingModel.Credit),
        OpeningBalance = 0,
        DefaultCreditLimit = 1500,
        CreditLimit = 1500,
        Balance = 0,
        CustomerId = "RVAT-1234567",
        ActivationDate = DateTime.Now,
        ExpirationDate = DateTime.Now,
        Open = true,
        Suspended = false,
        ProductCode = "VT-METRO1",
        Aliases = new List<String>() { "27878058095", "27878058096" },
        FollowMeNumbers = new List<AccountFollowMeViewModel>() { new AccountFollowMeViewModel() { Number = "27831234567" } },
        CellphoneNumber = "27831234567",
        RoutingPlan = "VDP",
        H323Password = "1a2b3c4d"
      };
      return accountViewModel;
    }


    /// <summary> Converts the string for a call forwarding options into the call forwarding option </summary>
    /// <param name="callForwardingOptions">String which represents a call forwarding option</param>
    /// <returns>The call forwarding option for the string</returns>
    private static CallForwardingOptions GetCallFowardingOption(string callForwardingOptions) {
      CallForwardingOptions result = CallForwardingOptions.NoForwarding;
      switch (callForwardingOptions) {
        case "F": result = CallForwardingOptions.AdvancedForwarding; break;
        case "Y": result = CallForwardingOptions.FollowMe; break;
        case "C": result = CallForwardingOptions.ForwardToCLD; break;
        case "U": result = CallForwardingOptions.ForwardToSIP; break;
      }
      return result;
    }

    /// <summary> Converts the enum for a call forwarding options into the string </summary>
    /// <param name="callForwardingOptions"></param>
    /// <returns>A String of CallForwardingOptions</returns>
    private static string GetCallFowardingOption(CallForwardingOptions callForwardingOptions) {
      string result = "N";
      switch (callForwardingOptions) {
        case CallForwardingOptions.AdvancedForwarding: result = "F"; break;
        case CallForwardingOptions.FollowMe: result = "Y"; break;
        case CallForwardingOptions.ForwardToCLD: result = "C"; break;
        case CallForwardingOptions.ForwardToSIP: result = "U"; break;
      }
      return result;
    }

    private static string CreateLocation(string number) {
      string result = "";
      int prefix = int.Parse(number.Substring(2, 2).ToString());
      if (prefix >= 10 && prefix < 60) {
        result = "LOC=" + prefix;
      }
      return result;
    }

    /// <summary> Gets the call forwarding string from the call forwarding option </summary>
    /// <param name="callForwardingOptions">Call forwarding option to convert</param>
    /// <returns>A string value for the given call forwarding option</returns>
    private static void GetCallFowardingString(AccountInfo account, CallForwardingOptions callForwardingOptions) {
      switch (callForwardingOptions) {
        case CallForwardingOptions.AdvancedForwarding:
          account.follow_me_enabled = "F";
          account.service_flags = "NL^^^NNN7^YN~N~NY^^NY~ N";
          break;

        case CallForwardingOptions.FollowMe:
          account.follow_me_enabled = "Y";
          account.service_flags = "NL^^^NNN7^YN~N~NY^^NY~ N";
          break;

        case CallForwardingOptions.ForwardToCLD:
          account.follow_me_enabled = "C";
          account.service_flags = "NL^^^NNN2^YN~N~NY^^NY~ N";
          break;

        case CallForwardingOptions.ForwardToSIP:
          account.follow_me_enabled = "U";
          account.service_flags = "NL^^^NNN7^YN~N~NY^^NY~ N";
          break;

        case CallForwardingOptions.NoForwarding:
          account.follow_me_enabled = "N";
          account.service_flags = "NL^^^NNN7^YN~N~NY^^NY~ N";
          break;
      }
    }

    /// <summary> Retrieves the Billing Model string for the given billing model identifier </summary>
    /// <param name="billingModelId">A PortaSwitch Billing Model Identifiker</param>
    /// <returns>A Billing Model string</returns>
    private static String GetBillingModel(int billingModelId) {
      BillingModel billingModel = (BillingModel)billingModelId;
      return billingModel.ToString();
    }

    /// <summary> Retrieves the Billing Model id for the given billing model </summary>
    /// <param name="billingModel">A Billing Model Identifiker</param>
    /// <returns>A Billing Model int</returns>
    private static int GetBillingModelString(string billingModel) {
      BillingModel result = Imagine.Rest.PortaSwitch.Account.BillingModel.Credit;
      switch (billingModel) {
        case "Debit": result = Imagine.Rest.PortaSwitch.Account.BillingModel.Debit; break;
        case "RechargeVoucher": result = Imagine.Rest.PortaSwitch.Account.BillingModel.RechargeVoucher; break;
        case "Credit": result = Imagine.Rest.PortaSwitch.Account.BillingModel.Credit; break;
        case "Alias": result = Imagine.Rest.PortaSwitch.Account.BillingModel.Alias; break;
      }
      return (int)result;
    }
  }
}