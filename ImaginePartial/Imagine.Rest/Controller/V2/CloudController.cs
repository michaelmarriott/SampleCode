using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Data;
using Imagine.Rest.ViewModel.Dr;

namespace Imagine.Rest.Controller.V2 {

   /// <summary> Clouds in Dr</summary>
  [RoutePrefix("2/Voice/Clouds")]
  public class CloudController : ApiController {

    /// <summary> Gets Clouds by Name</summary>
    /// <param name="name"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="orderby"></param>
    /// <returns>Array of Clouds</returns>
    [Route("")]
    [ResponseType(typeof(CloudViewModel[]))]
    public HttpResponseMessage GetAll(string name = "", int limit = 20, int offset = 0, string orderby = "cloudid") {
      List<CLOUD> clouds = new CLOUD().FindByName(name, limit, offset);
      return Request.CreateResponse(HttpStatusCode.OK, clouds.ToList().ConvertAll( e=> (CloudViewModel)e));
    }

  

    /// <summary> Gets cloud by id</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("{id}")]
    [ResponseType(typeof(CloudViewModel))]
    public HttpResponseMessage Get(int id) {
      var cloud = new CLOUD().Find(id);
      return Request.CreateResponse(HttpStatusCode.OK, (CloudViewModel)cloud);
    }


    /// <summary> </summary>
    /// <param name="cloud"></param>
    /// <returns></returns>
    [Route("")]
    public HttpResponseMessage Post(CloudViewModel cloudViewModel) {
       var cloud = (CLOUD)cloudViewModel;
       cloud.Create();
      return Request.CreateResponse(HttpStatusCode.OK, new { CloudId = cloud.CLOUDID });
    }

    /// <summary> </summary>
    /// <param name="id"></param>
    /// <param name="cloud"></param>
    /// <returns></returns>
    [Route("{id}")]
    [ResponseType(typeof(CloudViewModel))]
    public HttpResponseMessage Put(int id, CloudViewModel cloudViewModel) {
      var cloud = (CLOUD)cloudViewModel;
      cloud.Save();
      return Request.CreateResponse(HttpStatusCode.OK, (CloudViewModel)cloud);
    }

    /// <summary> </summary>
    /// <param name="id">id of Cloud</param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("{id}")]
    public HttpResponseMessage Delete(int id) {
      CLOUD cloud = new CLOUD().Find(id);
      if (cloud == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
      cloud.Delete();
      return Request.CreateResponse(HttpStatusCode.NotImplemented);
    }


  }
}