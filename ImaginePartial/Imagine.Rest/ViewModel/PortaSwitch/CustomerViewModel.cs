using log4net;
using System;
using System.ComponentModel.DataAnnotations;
using Imagine.Rest.Helper;
using Imagine.Rest.PortaSwitch.Customer;

namespace Imagine.Rest.ViewModel {

  /// <summary>
  /// Customer Model
  /// </summary>
  public class CustomerViewModel : BaseViewModel {

    #region Private Variables

    private static ILog log = LogManager.GetLogger(typeof(CustomerViewModel));

    #endregion Private Variables

    #region Properties

    /// <summary> Login for the customer [Unique, keep same as Id]</summary>
    [Required]
    public string Login { get; set; }

    /// <summary> Password for the customer </summary>
    public string Password { get; set; }

    /// <summary> Firstname of the customer </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary> Lastname of the customer </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary> Email address for the customer </summary>
    public string Email { get; set; }

    /// <summary> Customer salutation </summary>
    public string Salutation { get; set; }

    /// <summary> Indicates the status that the customer account is in [Open, Suspended, Closed], Default is Open </summary>
    public CustomerStatus Status { get; set; }

    /// <summary>  Bcc address</summary>
    public string Bcc { get; set; }

    /// <summary> Indicates the billing period for the customer [ Monthly or MonthlyAnniversary], Default is Monthly </summary>
    public BillingPeriod BillingPeriod { get; set; }

    /// <summary> Billing Date for monthly anniversary billing periods </summary>
    public DateTime ShiftedBillingDate { get; set; }

    /// <summary> Gets or sets the 3 character currency code.Default is ZAR </summary>
    /// <exception cref="System.ArgumentException">Throws an exception if a ISO code is used that is not three characters long</exception>
    public string ISOCurrency { get; set; }

    /// <summary> Reference to another CustomerEntity who owns this customer. </summary>
    [Required]
    public string ResellerIdentifier { get; set; }

    /// <summary> TimeZone Entry for Customer. Default is  </summary>
    /// <remarks>Time zone is read from the Time_Zones Table</remarks>
    public int TimeZoneID { get; set; }

    /// <summary> The type of customer [ Retail, Reseller, Distributor]. Retail is default </summary>
    public CustomerType Type { get; set; }

    /// <summary> Initial balance on the customer. This can only be set on new customers, it will be ignored once the customer is saved.</summary>
    public double OpeningBalance { get; set; }

    /// <summary> Maximum amount of Credit allowed before the customer is cut off </summary>
    public double CreditLimit { get; set; }

    /// <summary> Current balance for the Customer. This cannot be altered after the customer is created. (ID > 0)</summary>
    public double Balance { get; private set; }

    /// <summary> Percentage value that is used to warn the cusomter that they will exceed the limit </summary>
    public string WarningLimit { get; set; }

    /// <summary> Pastel Code for the customer </summary>
    public string PastelCode { get; set; }

    /// <summary> Billing reset day </summary>
    public int BillingResetDay { get; set; }

    /// <summary> Amount of extra credit allowed for the current billing cycle </summary>
    public double CreditExtension { get; set; }

    #endregion Properties

    /// <summary> Builds a Customer Entity from the Given Customer Info </summary>
    /// <param name="customer"> A Customer Info Structure </param>
    /// <returns> A customer entity </returns>
    public static explicit operator CustomerViewModel(CustomerInfo customer) {
      CustomerStatus status = CustomerStatus.Open;
      if (!customer.bill_status.Equals("O")) {
        status = customer.bill_status.Equals("S") ? CustomerStatus.Suspended : CustomerStatus.Closed;
      }
      var entity = new CustomerViewModel() {
        InternalId = customer.i_customer,
        Id = customer.name,
        ISOCurrency = customer.iso_4217,
        Balance = customer.balance,
        FirstName = customer.firstname,
        LastName = customer.lastname,
        Login = customer.login,
        Password = customer.password,
        Email = customer.email,
        Salutation = customer.salutation,
        TimeZoneID = customer.i_time_zone,
        Type = RetrieveCustomerType(customer.i_customer_type),
        BillingPeriod = customer.i_billing_period > 0 ? RetrieveBillingPeriod(customer.i_billing_period) : BillingPeriod.Monthly,
        OpeningBalance = customer.opening_balance,
        CreditLimit = customer.credit_limit == null ? 0 : (double)customer.credit_limit,
        Status = status,
        WarningLimit = customer.credit_limit_warning,
        Bcc = customer.bcc,
        ShiftedBillingDate = customer.shifted_billing_date
      };
      entity.ResellerIdentifier = (customer.i_parent > 0) ? new CustomerInfo().Find(customer.i_parent).name : string.Empty;
      entity.CreditExtension = customer.temp_credit_limit;
      //TODO : Check Porta for new functionality
      // entity.BillingResetDay = customerMaintenance.GetBillingDay(entity.InternalId, entity.Id);      
      return entity;
    }


    /// <summary> Builds a Customer Entity from the Given Customer Info </summary>
    /// <param name="customer"> A Customer Info Structure </param>
    /// <returns> A customer entity </returns>
    public static explicit operator CustomerInfo(CustomerViewModel customer) {
      var reseller = new CustomerInfo().Find(customer.ResellerIdentifier);
      if (reseller == null) { throw new ArgumentNullException("CustomerInfo", "Must provide a reseller when creating a customer entity."); }
      var resellerPrefix = reseller.name.Split(char.Parse("-"))[0];
      if (customer.TimeZoneID == 0)
        customer.TimeZoneID = 367;
      if (customer.ISOCurrency == null)
        customer.ISOCurrency = "ZAR";
      var first_of_month = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1);
      var entity = new CustomerInfo();
      entity.i_customer = customer.InternalId;
      entity.i_customerSpecified = customer.InternalId != 0;
      entity.name = customer.Id;
      entity.i_customer_class = reseller.i_customer_class;
      entity.i_customer_classSpecified = true;
      entity.iso_4217 = customer.ISOCurrency;
      entity.balance = (float)customer.Balance;
      entity.firstname = customer.FirstName;
      entity.lastname = customer.LastName;
      entity.login = customer.Login;
      entity.password = string.IsNullOrEmpty(customer.Password) ? RandomPasswordGenerator.CreateRandomPassword(12) : customer.Password;
      entity.email = customer.Email;
      entity.salutation = customer.Salutation;
      entity.i_time_zone = customer.TimeZoneID;
      entity.i_time_zoneSpecified = true;
      entity.i_customer_type = customer.Type != 0 ? RetrieveCustomerTypeId(customer.Type) : RetrieveCustomerTypeId(CustomerType.Retail);
      entity.i_customer_typeSpecified = true;
      entity.i_billing_period = customer.BillingPeriod != 0 ? RetrieveBillingPeriodId(customer.BillingPeriod) : RetrieveBillingPeriodId(BillingPeriod.Monthly);
      entity.i_billing_periodSpecified = true;
      entity.opening_balance = (float)customer.OpeningBalance;
      entity.opening_balanceSpecified = true;
      if ((int)customer.CreditExtension > 0) {
        entity.perm_credit_limit = (float)customer.CreditLimit;
        entity.perm_credit_limitSpecified = true;
        entity.temp_credit_limit = (int)customer.CreditExtension;
        entity.temp_credit_limitSpecified = (int)customer.CreditExtension != 0;
        entity.credit_limit_until = first_of_month.ToString("yyyy-MM-dd");
      }
      else {
        entity.credit_limit = (float)customer.CreditLimit;
        entity.credit_limitSpecified = customer.CreditLimit > 0;
      }
      entity.bill_status = RetrieveCustomerStatusId(customer.Status);
      entity.credit_limit_warning = customer.WarningLimit;
      entity.bcc = customer.Bcc;
      entity.shifted_billing_date = customer.ShiftedBillingDate;
      entity.shifted_billing_dateSpecified = customer.ShiftedBillingDate != new DateTime();
      entity.send_invoices = "N";
      entity.dialing_rules = new DialingRulesInfo() {
        cc = "27",
        ac = "87",
        dp = "0",
        ip = "00",
        em = "10111",
        ex = "6245,6246,*%"
      };
      entity.ppm_enabled = "N";
      entity.drm_enabled = "N";
      entity.out_date_format = "YYYY-MM-DD";
      entity.out_time_format = "HH24:MI:SS";
      entity.out_date_time_format = "YYYY-MM-DD HH24:MI:SS";
      entity.in_date_format = "YYYY-MM-DD";
      entity.in_time_format = "HH24:MI:SS";
      entity.send_statistics = "N";
      if (customer.ResellerIdentifier != null) {
        entity.i_parent = reseller.i_customer;
        entity.i_parentSpecified = true;
      }
      return entity;
    }


    /// <summary> Test Model Generator for Documentation </summary>
    /// <returns> A Test Object of CustomerViewModel </returns>
    static public CustomerViewModel TestModel() {
      var customerViewModel = new CustomerViewModel() {
        Login = "JDoe",
        Password = "xxxxxx",
        FirstName = "John",
        LastName = "Doe",
        Email = "johndoe@vox.co.za",
        Salutation = "",
        Status = Imagine.Rest.PortaSwitch.Customer.CustomerStatus.Open,
        Bcc = "",
        BillingPeriod = BillingPeriod.Monthly,
        ShiftedBillingDate = DateTime.Now,
        ISOCurrency = "R",
        ResellerIdentifier = "0",
        TimeZoneID = 1,
        Type = CustomerType.Retail,
        OpeningBalance = 0,
        CreditLimit = 1500,
        Balance = 0,
        WarningLimit = "90",
        PastelCode = "ABC123",
        BillingResetDay = 1,
        CreditExtension = 1700
      };
      return customerViewModel;
    }

    /// <summary> Retrieves the CustomerType for the given PortaSwitch customer type identifier </summary>
    /// <param name="customerTypeId"> Integer Identifier which represents the customer type </param>
    /// <returns> Customer Type </returns>
    private static CustomerType RetrieveCustomerType(int customerTypeId) {
      CustomerType customerType = (CustomerType)customerTypeId;
      return customerType;
    }

    /// <summary> Retrieves the integer identifier value for the given customer type </summary>
    /// <param name="customerType">String Identifier which represents the customer type </param>
    /// <returns> Integer Identifier name of the customer type </returns>
    private static int RetrieveCustomerTypeId(CustomerType customerType) {
      int typeId = (int)customerType;
      return typeId;
    }

    /// <summary> Retrieve the billing period string for the given BillingPerio identifier </summary>
    /// <param name="billingPeriodId">BillingPeriod Identifier</param>
    /// <returns>String Identifier</returns>
    private static BillingPeriod RetrieveBillingPeriod(int billingPeriodId) {
      BillingPeriod billingPeriod = (BillingPeriod)billingPeriodId;
      return billingPeriod;
    }

    /// <summary> Retrieve the PortaSwitch identifier for the given billing period </summary>
    /// <param name="billingPeriod">BillingPeriod Identifier</param>
    /// <returns>Integer Identifier</returns>
    private static int RetrieveBillingPeriodId(BillingPeriod billingPeriod) {
      int billingPeriodId = (int)(billingPeriod);
      return billingPeriodId;
    }

    /// <summary> Retrieve the PortaSwitch identifier for the given customer status </summary>
    /// <param name="customerStatus">CustomerStatus</param>
    /// <returns>Integer Identifier</returns>
    private static string RetrieveCustomerStatusId(CustomerStatus customerStatus) {
      if (customerStatus == CustomerStatus.Open) {
        return "O";
      }
      else if (customerStatus == CustomerStatus.Suspended) {
        return "S";
      }
      else if (customerStatus == CustomerStatus.Closed) {
        return "C";
      }
      else {
        return "O";
      }
    }


  }
}