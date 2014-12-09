using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.PortaSwitch.DID;
using Imagine.Rest.ViewModel;

namespace Imagine.Rest.Controller.V2 {

  /// <summary> Voice Did Number Service</summary>
  //  [ApiExplorerSettings(IgnoreApi = true)]
  [RoutePrefix("2/Voice/VoiceDidNumbers")]
  public class VoiceDIDNumbersController : ApiController {

    #region Put
    /// <summary> Reserves a DID Number.  </summary>
    /// <param name="didNumberReserveViewModel">DidNumber Reserve Model</param>
    /// <returns>Status Code 200 if the account is succesfully updated, or a 400 if the information is incorrect</returns>
    [Route("Reserve")]
    [ResponseType(typeof(List<String>))]
    public HttpResponseMessage PutReserve(DIDNumberReserveViewModel didNumberReserveViewModel) {
      var didNumberInfo = new DIDNumberInfo();
      var result = didNumberInfo.Reserve(didNumberReserveViewModel.ResellerIdentifier, didNumberReserveViewModel.Amount, didNumberReserveViewModel.IsBlockSet, didNumberReserveViewModel.Prefix);
      if (result.Count != didNumberReserveViewModel.Amount) {
        return Request.CreateResponse(HttpStatusCode.NotFound);
      }
      return Request.CreateResponse(HttpStatusCode.OK, result);
    }

    /// <summary> Retrieves all of the avaialable phone numbers</summary>
    /// <param name="resellerIdentifier">Reseller name that the search will be restricted</param>
    /// <param name="amount">Amount of numbers to look for (default = 1) [optional]</param>
    /// <param name="prefix">Prefix for the number [optional]</param>
    /// <param name="limit">Limit the number of customers to return (Default = 10)</param>
    /// <param name="offset">Offset from the 0 the customers to return (Default = 0)</param>
    /// <returns>List of numbers returned, Status Code 200 if the account is succesfully updated, or a 400 if the information is incorrect</returns>
    [Route("Reserve")]
    [ResponseType(typeof(List<String>))]
    public HttpResponseMessage Get(string resellerIdentifier, int amount = 1, bool isBlockSet = false, string prefix = "", int limit = 10, int offset = 0) {
      var didNumberInfo = new DIDNumberInfo();
      var result = didNumberInfo.Find(resellerIdentifier, amount, isBlockSet, prefix, limit, offset);
      return Request.CreateResponse(HttpStatusCode.OK, result);
    }
    #endregion

  }
}