using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.Data;
using Imagine.Rest.Model.Voxzal;
using Imagine.Rest.ViewModel.Voxzal;

namespace Imagine.Rest.Controller.V2 {

  /// <summary> Provides the resource for Voice Jobs</summary>
  [RoutePrefix("2/Voice/VoiceJobs")]
  public class VoiceJobsController : ApiController {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    [Route("")]
    [ResponseType(typeof(VoiceJobsViewModel[]))]
    public HttpResponseMessage GetAll(string name, int limit = 20, int offset = 0) {
      var voiceJobs = new voice_job().FindAll(name, limit, offset);
      return Request.CreateResponse(HttpStatusCode.OK, voiceJobs.ToList().ConvertAll(e => (VoiceJobsViewModel)e));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("{id}")]
    [ResponseType(typeof(VoiceJobsViewModel))]
    public HttpResponseMessage Get(Guid id) {
      var voiceJob = new voice_job().Find(id);
      return Request.CreateResponse(HttpStatusCode.OK, (VoiceJobsViewModel)voiceJob);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="voiceJobsViewModel"></param>
    /// <returns></returns>
    [Route("")]
    public HttpResponseMessage Post([FromBody]VoiceJobsViewModel voiceJobsViewModel) {
      if (voiceJobsViewModel.LastUpdated == null) { voiceJobsViewModel.LastUpdated = DateTime.Now; }
      var voiceJobs = (voice_job)voiceJobsViewModel;
      voiceJobs.Create();
      return Request.CreateResponse(HttpStatusCode.Created, new { id = voiceJobs.id });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="voiceJobsViewModel"></param>
    /// <returns></returns>
    [Route("{id}")]
    [ResponseType(typeof(VoiceJobsViewModel))]
    public HttpResponseMessage Put(Guid id, [FromBody]VoiceJobsViewModel voiceJobsViewModel) {
      var voiceJob = (voice_job)voiceJobsViewModel;
      if (id != voiceJob.id) { return Request.CreateResponse(HttpStatusCode.BadRequest); }
      voiceJob.Save();
      return Request.CreateResponse(HttpStatusCode.OK, voiceJobsViewModel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="voiceJobsViewModel"></param>
    /// <returns></returns>
    [Route("{id}")]
    public HttpResponseMessage Patch(Guid id, [FromBody]dynamic voiceJobsViewModel) {
      var voiceJob = new voice_job().Find(id);
      foreach (var p in voiceJobsViewModel.GetType().GetProperties()) {
        voiceJob.GetType().GetProperty(p.Key).SetValue(p.value);
      }
      voiceJob.Save();
      return Request.CreateResponse(HttpStatusCode.OK);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("{id}")]
    public HttpResponseMessage Delete(Guid id) {
      var voiceJob = new voice_job().Find(id);
      voiceJob.Delete();
      return Request.CreateResponse(HttpStatusCode.OK, (VoiceJobsViewModel)voiceJob);
    }

  }
}