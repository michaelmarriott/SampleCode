using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Model.Dr;

namespace Imagine.Rest.Controller.V2 {

  /// <summary>Dr Tariffs</summary>
    [RoutePrefix("2/Voice/Tariff")]
  public class TariffController : ApiController {

    /// <summary>Retrieves ucdr tariffs by callplanId and/or zones</summary>
    /// <param name="callplanId">Call plan id</param>
    /// <param name="zones">Different zones [numeric string serperated by commas]</param>
    /// <returns>TariffSummary</returns>
    [Route("")]
    [ResponseType(typeof(TariffSummary[]))]
    public HttpResponseMessage GetBy(string callplanId, string zones = "1,2,3,4,5,6,7,8,10,11,100") {
      try {
        var tariff = new TariffSummary().Find(callplanId, zones);
        if (tariff == null) {
          var message = string.Format("GET TARIFF BY CALLPLAN NOT FOUND");
          return Request.CreateResponse(HttpStatusCode.NotFound, message);
        }
        return Request.CreateResponse(HttpStatusCode.OK, tariff);
      } catch (Exception ex) {
        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
      }
    }

  }
}