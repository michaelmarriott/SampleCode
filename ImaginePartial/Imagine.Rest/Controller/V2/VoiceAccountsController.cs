using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Model.PortaSwitch;
using Imagine.Rest.PortaSwitch.Account;
using Imagine.Rest.PortaSwitch.Customer;
using Imagine.Rest.PortaSwitch.VoiceMail;
using Imagine.Rest.ViewModel;

namespace Imagine.Rest.Controller.V2 {

  /// <summary> Provides the resource for the Voice Accounts </summary>
  [RoutePrefix("2/Voice/VoiceAccounts")]
  public class VoiceAccountsController : ApiController {

    #region Get

    /// <summary> Retrieves a Voice Acccount for the Given Phone Number &lt;/br&gt;
    /// Replaces: &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/Imagine.Request.Accounts.GetByNumber" &gt;VOX/VOICE/REQUEST/ACCOUNTS/GETBYNUMBER&lt;/a&gt;
    /// </summary>
    /// <param name="id">Account Identifier for the Voice Account (Should normally be a phone number)</param>
    /// <returns>A Account Model if found, otherwise it will return a not found status code (404)</returns>
    [Route("{id}")]
    [ResponseType(typeof(AccountViewModel))]
    public HttpResponseMessage GetById(string id) {
      AccountInfo accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      return Request.CreateResponse(HttpStatusCode.OK, (AccountViewModel)accountInfo);
    }

    #endregion Get

    #region Post
    /// <summary> Adds the account with the provided Model Info. This is called by provisioning and is part of workflow steps.</summary>
    /// <param name="account">The Account Model</param>
    /// <returns>tatus Code 201 if the account is succesfully created, or a 400 if the information is incorrect </returns>
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ResponseType(typeof(HttpResponseMessage))]
    public HttpResponseMessage PostAccount(AccountViewModel account) {
      if (account == null) {
        return Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Must set the Account Property in order to create."));
      }
      var accountInfo = (AccountInfo)account;
      if (accountInfo.Create()) {
        return Request.CreateResponse(HttpStatusCode.Created, new {
          Id = account.Id
        });
      } else {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }
    }

    #endregion

    #region Put

    /// <summary> Updates the account with the provided Model information &lt;/br&gt;
    /// Replaces: &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/Imagine.Suspend" &gt;VOX/VOICE/SUSPEND&lt;/a&gt;
    /// &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/Imagine.Account.CallForwarding.Add" &gt;VOX/VOICE/ACCOUNT/CALLFORWARDING/ADD&lt;/a&gt;
    /// &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/Imagine.Account.ConsumptionLimit" &gt;VOX/VOICE/ACCOUNT/CONSUMPTIONLIMIT&lt;/a&gt;
    /// </summary>
    /// <param name="id">Account Identifier (PhoneNumber)</param>
    /// <param name="account">Account Information for the update</param>
    /// <returns>Status Code 200 if the account is succesfully updated, or a 400 if the information is incorrect</returns>
    [Route("{id}")]
    [ResponseType(typeof(AccountViewModel))]
    public HttpResponseMessage PutAccount(string id, AccountViewModel account) {
      if (account == null) 
        return Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Must set the Account Property in order to update."));
      
      var accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) 
        return Request.CreateResponse(HttpStatusCode.NotFound, "Account " + id + " Not Found");

      if (account.Id != null && account.Id != id)
        return Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Id has unexepcted value exepcted ({0} = {1}).", account.Id, id));

      if (account.InternalId !=0 && account.InternalId != accountInfo.i_account) 
        return Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("InternalId has unexepcted value exepcted ({0} = {1}).", account.InternalId, accountInfo.i_account));

      account.Id = id;
      account.InternalId = accountInfo.i_account;
      if (account.ProductCode == null) 
        account.ProductId = accountInfo.i_product;
      if (account.CustomerId == null)
        account.CustomerId = accountInfo.customer_name;
      if (account.RoutingPlan == null)
        account.RoutingPlan = new RoutingPlanInfo().GetRoutesById(accountInfo.i_routing_plan).Name;

      accountInfo = (AccountInfo)account;
      if (accountInfo.Save()) {
        return Request.CreateResponse(HttpStatusCode.OK, (AccountViewModel)accountInfo);
      } else {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }
    }

    /// <summary> Moves the account Id to a new customer </summary>
    /// <param name="id"></param>
    /// <param name="voiceCustomerId"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("{id}/Move/{voiceCustomerId}")]
    [ResponseType(typeof(HttpResponseMessage))]
    public HttpResponseMessage PutAccountMove(string id, string voiceCustomerId) {
      AccountInfo accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound, "Account " + id + " Not Found");
      }
      CustomerInfo customer = new CustomerInfo().Find(voiceCustomerId);
      if (customer == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound, "Customer " + voiceCustomerId + " Not Found");
      }
 
      accountInfo.i_customer = customer.i_customer;
      if (accountInfo.Move()) {
        return Request.CreateResponse(HttpStatusCode.OK, new {
          Id = id
        });
      } else {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }
    }

    #endregion Put

    #region Delete
    /// <summary> Deletes the Account</summary>
    /// <param name="id">Account Identifier for the Voice Account (Should normally be a phone number)</param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("{id}")]
    public HttpResponseMessage DeleteAccount(string id) {
      AccountInfo accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound, "Account " + id + " Not Found");
      }
      if (accountInfo.Delete()) {
        return Request.CreateResponse(HttpStatusCode.OK);
      } else {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }
    }
    #endregion

    #region Aliases

    #region Get

    /// <summary>
    /// Gets the Aliases for the Current Voice account. Will return an empty result if no accounts could be found
    /// </summary>
    /// <param name="id">Phone Number to retrieve the Aliases for</param>
    /// <returns>A Array of Aliases if any exist, otherwise an empty Array </returns>
    [Route("{id}/aliases/")]
    public HttpResponseMessage GetAliasesById(string id) {
      AccountInfo accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      var aliases = accountInfo.LoadAliases();
      return Request.CreateResponse(HttpStatusCode.OK, aliases);
    }

    #endregion

    #endregion

    #region VoiceMails

    #region Get

    /// <summary> Retrieves a Voice Acccount Voicemail for the Given Phone Number &lt;/br&gt;
    /// </summary>
    /// <param name="id">Account Identifier for the Voice Account (Should normally be a phone number)</param>
    /// <returns>A Account Model if found, otherwise it will return a not found status code (404)</returns>
    [Route("{id}/Voicemails/")]
    [ResponseType(typeof(VoiceMailViewModel))]
    public HttpResponseMessage GetVoicemailById(string id) {
      VoiceMailViewModel voiceMailViewModel = null;
      AccountInfo accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      if (accountInfo.um_enabled.Equals("Y")) {
        var vMSettings = new VMSettings() {
          AccountPassword = accountInfo.password
        };
        var voicemail = vMSettings.Find(accountInfo.id);
        string pin = null;
        if (voicemail != null)
          pin = voicemail.password;
        voiceMailViewModel = new VoiceMailViewModel() {
          Enabled = true,
          Pin = pin
        };
      } else {
        voiceMailViewModel = new VoiceMailViewModel() {
          Enabled = false
        };
      }
      return Request.CreateResponse(HttpStatusCode.OK, (VoiceMailViewModel)voiceMailViewModel);
    }

    #endregion

    #region Put
    /// <summary> Updates the VoiceMail Settings for the Voice Account &lt;/br&gt;
    /// Replaces: &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/VoiceMail" &gt;PROVISIONING/VOICEMAIL &lt;/a&gt;
    /// </summary>
    /// <param name="id">Account Identifier (PhoneNumber)</param>
    /// <param name="voiceMailViewModel"> VoiceMail View Model which contains the Pin </param>
    /// <returns>Status Code 200 if the account voice mail is succesfully update, or 400 or 404 if incorrect</returns>
    [Route("{id}/VoiceMails")]
    [ResponseType(typeof(VoiceMailViewModel))]
    public HttpResponseMessage PutAccountVoiceMail(string id, VoiceMailViewModel voiceMailViewModel) {
      if (voiceMailViewModel == null) {
        return Request.CreateResponse(HttpStatusCode.BadRequest, String.Format("Must set the VoiceMail Property in order to update."));
      }
      AccountInfo accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound, "Account " + id + " Not Found");
      }
      accountInfo.um_enabled = voiceMailViewModel.Enabled ? "Y" : "N";
      accountInfo.Save();

      var vmSettings = (VMSettings)voiceMailViewModel;
      vmSettings.AccountId = accountInfo.id;
      vmSettings.AccountPassword = accountInfo.password;

      if (vmSettings.Save()) {
        return Request.CreateResponse(HttpStatusCode.OK, voiceMailViewModel);
      } else {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }
    }

    #endregion

    #endregion

    #region FollowMeNumbers

    #region Get

    /// <summary>
    /// Retrieves the follow me numbers for the Account. Follow me numbers are used when the Call Forwarding Option for the account is enabled
    /// </summary>
    /// <param name="id">Account Number to retrieve follow me numbers for</param>
    /// <returns>A Array of FollowMeNumbers configured for the Account</returns>
    [Route("{id}/FollowMeNumbers")]
    [ResponseType(typeof(AccountFollowMeViewModel[]))]
    public HttpResponseMessage GetFollowMeNumbers(string id) {
      var accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      FollowMeNumberInfo[] followMeNumbers = accountInfo.LoadFollowMeNumbers();
      if (followMeNumbers.Length == 0) {
        return Request.CreateResponse(HttpStatusCode.NoContent);
      }
      return Request.CreateResponse(HttpStatusCode.OK, followMeNumbers.ToList().ConvertAll(e => (AccountFollowMeViewModel)e));
    }

    /// <summary>
    /// Retrieves the follow me numbers for the Account. Follow me numbers are used when the Call Forwarding Option for the account is enabled
    /// </summary>
    /// <param name="id">Account Number to retrieve follow me numbers for</param>
    /// <param name="followMeId">Follow Me Identifier to use for the Follow Me information</param>
    /// <returns>A Array of FollowMeNumbers configured for the Account</returns>
    [Route("{id}/FollowMeNumbers/{followMeId}")]
    [ResponseType(typeof(AccountFollowMeViewModel))]
    public HttpResponseMessage GetFollowMeNumberById(string id, string followMeId) {
      var accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      FollowMeNumberInfo followMeNumber = GetFollowMeNumberById(followMeId, accountInfo);
      if (followMeNumber == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      return Request.CreateResponse(HttpStatusCode.OK, (AccountFollowMeViewModel)followMeNumber);
    }

    private FollowMeNumberInfo GetFollowMeNumberById(string followMeId, AccountInfo accountInfo) {
      FollowMeNumberInfo[] followMeNumbers = accountInfo.LoadFollowMeNumbers();
      FollowMeNumberInfo followMeNumber = null;
      for (int i = 0; i < followMeNumbers.Length && followMeNumber == null; i++) {
        if (followMeNumbers[i].i_follow_me_number.ToString().Equals(followMeId)) {
          followMeNumber = followMeNumbers[i];
        }
      }
      return followMeNumber;
    }

    #endregion

    #region Post

    /// <summary>
    /// Creates a follow me numbers for the Account. Follow me numbers are used when the Call Forwarding Option for the account is enabled
    /// </summary>
    /// <param name="id">Account Number to retrieve follow me numbers for</param>
    /// <param name="followMeNumber">Follow me number Model</param>
    /// <returns>A Array of FollowMeNumbers configured for the Account</returns>
    [Route("{id}/FollowMeNumbers")]
    [ResponseType(typeof(HttpResponseMessage))]
    public HttpResponseMessage PostAddFollowMeNumber(string id, AccountFollowMeViewModel followMeNumber) {
      var accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      followMeNumber.AccountInternalId = accountInfo.i_account;
      followMeNumber.Id = accountInfo.AddFollowMeNumber((FollowMeNumberInfo)followMeNumber).ToString();
      return Request.CreateResponse(HttpStatusCode.OK, new {
        id = followMeNumber.Id
      });
    }

    #endregion

    #region Delete

    /// <summary>
    /// Removes the follow me number for the Account. Follow me numbers are used when the Call Forwarding Option for the account is enabled
    /// </summary>
    /// <param name="id">Account Number to retrieve follow me numbers for</param>
    /// <param name="followMeId">Follow me number Identifier</param>
    /// <returns>A Array of FollowMeNumbers configured for the Account</returns>
    [Route("{id}/FollowMeNumbers/{followMeId}")]
    [ResponseType(typeof(AccountFollowMeViewModel))]
    public HttpResponseMessage DeleteFollowMeNumber(string id, string followMeId) {
      var accountInfo = new AccountInfo().Find(id);
      if (accountInfo == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      FollowMeNumberInfo followMeNumber = GetFollowMeNumberById(followMeId, accountInfo);
      if (followMeNumber == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      bool result = accountInfo.DeleteFollowMeNumber(followMeNumber.i_follow_me_number);
      return Request.CreateResponse(HttpStatusCode.OK, (AccountFollowMeViewModel)followMeNumber);
    }

    #endregion

    #endregion
  }
}