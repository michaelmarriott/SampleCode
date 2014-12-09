using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Data;
using Imagine.Rest.ViewModel.Dr;

namespace Imagine.Rest.Controller.V2 {

  /// <summary>
  /// The Monthly Billruns for each Lin of Business (LOB)
  /// </summary>
   [ApiExplorerSettings(IgnoreApi = true)]
  [RoutePrefix("2/Voice/BillRun")]
  public class BillRunController : ApiController {

    [Route("")]
    [ResponseType(typeof(BillRunViewModel[]))]
    public HttpResponseMessage GetAll(int lobId=0,string code=null,int? isMidMonth =null, int limit = 20, int offset = 0) {
      var billruns = new BILLRUN().FindAll(lobId, code, isMidMonth, limit, offset);
      return Request.CreateResponse(HttpStatusCode.OK, billruns.ToList().ConvertAll(e => (BillRunViewModel)e));
    }

    [Route("{id}")]
    [ResponseType(typeof(BillRunViewModel))]
    public HttpResponseMessage Get(int id) {
      var billrun = new BILLRUN().Find(id);
      return Request.CreateResponse(HttpStatusCode.OK, (BillRunViewModel)billrun);
    }

    [Route("")]
    public HttpResponseMessage Post([FromBody]BillRunViewModel billRunViewModel) {
      var billrun = (BILLRUN)billRunViewModel;
      billrun.Create();
      return Request.CreateResponse(HttpStatusCode.Created, new { id = billrun.BILLRUNID });
    }

    [Route("{id}")]
    [ResponseType(typeof(BillRunViewModel))]
    public HttpResponseMessage Put(int id, [FromBody]BillRunViewModel billRunViewModel) {
      var billrun = (BILLRUN)billRunViewModel;
      if (id != (int)billrun.BILLRUNID) { return Request.CreateResponse(HttpStatusCode.BadRequest); }
      billrun.Save();
      return Request.CreateResponse(HttpStatusCode.OK, billRunViewModel);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("{id}")]
    public HttpResponseMessage Delete(int id) {
      return Request.CreateResponse(HttpStatusCode.NotImplemented);
    }

  }
}