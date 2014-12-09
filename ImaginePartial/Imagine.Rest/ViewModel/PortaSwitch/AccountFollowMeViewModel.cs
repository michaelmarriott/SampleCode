using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Imagine.Rest.PortaSwitch.Account;

namespace Imagine.Rest.ViewModel {

  /// <summary>
  /// Represents the Account Follow Me numbers (Call forward number)
  /// </summary>
  public class AccountFollowMeViewModel : BaseViewModel {

    #region Properties


    /// <summary> Gets or sets the Internal Account Reference Identifier (PortaSwitch Internal Identifier) </summary>
    [IgnoreDataMember]
    public int AccountInternalId { get; set; }

    /// <summary> Gets or sets the Phone number for the call forward action </summary>
    [Required]
    public string Number { get; set; }

    /// <summary> Gets or sets the period during which the number can be used [Default:Always]</summary>
    //public string Period { get; set; }

    /// <summary> Gets or sets the Description of the period which the number can be used [Default:Always]</summary>
    // public string PeriodDescription { get; set; }

    #endregion Properties

    /// <summary>
    /// Converts a FollowMeNumberInfo into a AccountFollowMeViewModel
    /// </summary>
    /// <param name="info">FollowMeNumberInfo Object</param>
    /// <returns>A AccountFollowMeViewModel</returns>
    public static explicit operator AccountFollowMeViewModel(FollowMeNumberInfo info) {
      return new AccountFollowMeViewModel() {
        Id = info.i_follow_me_number.ToString(),
        InternalId = info.i_follow_me_number,
        Number = info.redirect_number,
        AccountInternalId = info.i_account
      };
      
    }

    /// <summary>
    /// Converts a AccountFollowMeViewModel into a FollowMeNumberInfo
    /// </summary>
    /// <param name="model">AccountFollowMeViewModel Object</param>
    /// <returns>A FollowMeNumberInfo</returns>
    public static explicit operator FollowMeNumberInfo(AccountFollowMeViewModel model) {
      return new FollowMeNumberInfo() {
        i_account = model.AccountInternalId,
        i_accountSpecified = model.AccountInternalId > 0,
        name = model.InternalId.ToString(),
        redirect_number = model.Number,
        period = "Always",
        period_description = "Always"
      };
    }

    /// <summary> Test Model Generator for Documentation </summary>
    /// <returns>A Test Object of VoiceMailViewModel</returns>
    static public AccountFollowMeViewModel TestModel() {
      var followMeNumber = new AccountFollowMeViewModel() {
        Number = "27871234567"
      };
      return followMeNumber;
    }
  }
}