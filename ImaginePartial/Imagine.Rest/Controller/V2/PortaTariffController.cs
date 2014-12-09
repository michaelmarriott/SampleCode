using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Model.Dr;


namespace Imagine.Rest.Controller.V2 {

  /// <summary>Dr Tariffs in Porta formatting</summary>
  [RoutePrefix("2/Voice/PortaTariff")]
  public class PortaTariffController : ApiController {

    /// <summary>
    ///  Retrieves outbound call data records for lob/customer/account/devices 
    /// </summary>
    /// <param name="lobId">1=VoxTelecom, 2=Wholesale, 3=@lantic, 4=VoxTelepreneur, 5=Core, 6=Orion</param>
    /// <param name="dateStart">Date to start request from</param>
    /// <param name="dateEnd">Date to end request to</param>
    /// <param name="customerId">The customer guid</param>
    /// <param name="accountcode">The customer account code</param>
    /// <param name="deviceIdList">String delimited devices</param>
    /// <param name="offset">Start from record</param>
    /// <param name="limit">Amount of records to return</param>
    /// <returns>OutboundCallDataRecord</returns>
    [Route("")]
    [ResponseType(typeof(TariffSummary[]))]
    public HttpResponseMessage GetBy(String DialCodeGroupName, String IsoDialCode, String TimeCategory, double FirstPeriod, double FirstCharge, double NextPeriod, double NextCharge) {
      try {
        var tarrifSummarys = new TariffSummary();
        //	var tarrifSummarys = new TarrifSummary().Find(callplanId, fromEmail, toEmail, subject);

        if (tarrifSummarys == null) {
          var message = string.Format("Porta Tarrif's not found");
          return Request.CreateResponse(HttpStatusCode.NotFound, message);
        }
        return Request.CreateResponse(HttpStatusCode.OK, tarrifSummarys);
      } catch (Exception ex) {
        return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
      }
    }

  }
}