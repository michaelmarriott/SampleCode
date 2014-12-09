using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Data;
using Imagine.Rest.Model.Dr;
using Imagine.Rest.ViewModel.Dr;

namespace Imagine.Rest.Controller.V2 {

  /// <summary> Flags in Dr, these are used to run rating and data imports checks </summary>
  [ApiExplorerSettings(IgnoreApi = true)]
  [RoutePrefix("2/Voice/Flags")]
  public class FlagsController : ApiController {

    /// <summary> Gets Flags by Name</summary>
    /// <param name="name"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="orderby"></param>
    /// <returns>Array of Flags</returns>
    [Route("")]
    [ResponseType(typeof(FlagViewModel[]))]
    public HttpResponseMessage Get(string name = "", int limit = 20, int offset = 0, string orderby = "flagid") {
      var flags = new FLAG().FindByName(name, limit, offset, orderby);
      return Request.CreateResponse(HttpStatusCode.OK, flags.ToList().ConvertAll( e=> (FlagViewModel)e));
    }

   

    /// <summary> </summary>
    /// <param name="flagViewModel"></param>
    /// <returns></returns>
    [Route("")]
    public HttpResponseMessage Post(FlagViewModel flagViewModel) {
      if (flagViewModel.Status == null)
        flagViewModel.Status = 0;
      var flag = (FLAG)flagViewModel;
      flag.Create();
      return Request.CreateResponse(HttpStatusCode.OK, new { FlagId = (int)flag.FLAGID });
    }

    /// <summary> </summary>
    /// <param name="id"></param>
    /// <param name="flagViewModel"></param>
    /// <returns></returns>
    [Route("{id}")]
    [ResponseType(typeof(FlagViewModel))]
    public HttpResponseMessage Put(int id, FlagViewModel flagViewModel) {
      var flag = (FLAG)flagViewModel;
      flag.Save();
      return Request.CreateResponse(HttpStatusCode.OK, (FlagViewModel)flag);
    }

    /// <summary> </summary>
    /// <param name="id">id of Flag</param>
    /// <returns></returns>
    [Route("{id}")]
    [ResponseType(typeof(FlagViewModel))]
    public HttpResponseMessage Delete(int id) {
      FLAG flag = new FLAG().Find(id);
      if (flag == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
      flag.Delete();
      return Request.CreateResponse(HttpStatusCode.OK, (FlagViewModel)flag);
    }

  }
}