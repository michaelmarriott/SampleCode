using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Data;
using Imagine.Rest.Model.Voxzal;
using Imagine.Rest.ViewModel.Voxzal;

//http://cl.ly/JqXO -- MySql MODEL PLUGIN
// also install http://dev.mysql.com/downloads/connector/net/#downloads

namespace Imagine.Rest.Controller.V2 {

  /// <summary>Consolidated Itemized Billing</summary>
  [RoutePrefix("2/Voice/ConsolidatedItemizedBilling")]
  public class ConsolidatedItemizedBillingController : ApiController {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="consolidatedItemizedBillingRequest"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public HttpResponseMessage PostRequest(ConsolidatedItemizedBillingRequest consolidatedItemizedBillingRequest) {
      try {
        var consolidatedItemizedBilling = new ConsolidatedItemizedBilling();
       
        return Request.CreateResponse(HttpStatusCode.OK, new { });
      } catch (Exception ex) {
        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
      }
    }
  }
}