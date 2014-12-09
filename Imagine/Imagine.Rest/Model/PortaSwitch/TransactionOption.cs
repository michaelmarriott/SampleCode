using System.Runtime.Serialization;

namespace Imagine.Rest.PortaSwitch {

  #region Enums

  /// <summary>
  /// Enum of the possible Transaction options
  /// </summary>
  [DataContract]
  public enum TransactionOption {

    [EnumMember]
    ManualCharge = 1,

    [EnumMember]
    ManualRefund = 2,

    [EnumMember]
    ManualPayment = 3,

    [EnumMember]
    PromotionalCredit = 4,

    [EnumMember]
    ECommercePayment = 5,

    [EnumMember]
    ECommerceRefund = 6,

    [EnumMember]
    AuthorizationOnly = 7,

    [EnumMember]
    CapturePayment = 8,

    [EnumMember]
    ManualCredit = 9,

    [EnumMember]
    Refund = 10
  }

  #endregion Enums

  #region Helper Methods

  /// <summary>
  /// Helpers for the Transaction Objects used in the WebService
  /// </summary>
  public class TransactionHelpers {

    /// <summary>
    /// Translates the Transaction Option enumeration into the string that must be passed into the webservice
    /// </summary>
    /// <param name="option">The Transaction Option enumeration to pass</param>
    /// <returns>A string value which is used in the web service calls</returns>
    public static string GetTransactionString(TransactionOption option) {
      string transaction = "";
      switch (option) {
        case TransactionOption.ManualCharge:
          transaction = "Manual charge"; break;
        case TransactionOption.ManualRefund:
          transaction = "Manual refund"; break;
        case TransactionOption.ManualPayment:
          transaction = "Manual payment"; break;
        case TransactionOption.PromotionalCredit:
          transaction = "Promotional credit"; break;
        case TransactionOption.ECommercePayment:
          transaction = "e-commerce payment"; break;
        case TransactionOption.ECommerceRefund:
          transaction = "e-commerce refund"; break;
        case TransactionOption.AuthorizationOnly:
          transaction = "Authorization only"; break;
        case TransactionOption.CapturePayment:
          transaction = "Capture payment"; break;
        case TransactionOption.ManualCredit:
          transaction = "Manual credit"; break;
        case TransactionOption.Refund:
          transaction = "Refund"; break;
      }
      return transaction;
    }
  }

  #endregion Helper Methods

  /// <summary>
  /// Contains the information required to make a transaction on the Customer or Account entity.
  /// </summary>
  /// <remarks>This Entity is not like the other entities in that it does not inherit from Entity</remarks>
  [DataContract]
  public class TransactionEntity {

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionEntity" /> class.
    /// </summary>
    public TransactionEntity() {
    }

    #endregion Constructor

    #region Properties

    /// <summary> Gets or sets the Option to use when executing this transaction </summary>
    [DataMember]
    public TransactionOption Option { get; set; }

    /// <summary> Gets or sets the Amount to use for the transaction </summary>
    [DataMember]
    public double Amount { get; set; }

    /// <summary> Gets or sets the Comment visible in the PortaOne system </summary>
    [DataMember]
    public string VisibleComment { get; set; }

    /// <summary> Gets or sets the Comment visible via the PortaOne web service </summary>
    [DataMember]
    public string InternalComment { get; set; }

    /// <summary> Gets or sets the Stops the PortaOne service from sending the </summary>
    [DataMember]
    public int SuppressNotification { get; set; }

    #endregion Properties
  }
}