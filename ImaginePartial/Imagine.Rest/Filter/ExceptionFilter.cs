using log4net;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Imagine.Rest.Filter {

  public class ExceptionFilter : ExceptionFilterAttribute {

    /// <summary> Logger Instance </summary>
    private static ILog log = LogManager.GetLogger(typeof(ExceptionFilter));


    /// <summary> All Exceptions will pass through here and not through the log filter </summary>
    /// <param name="context"></param>
    public override void OnException(HttpActionExecutedContext context) {
      if (context.Exception is HttpResponseException) {
        context.Response = ((HttpResponseException)context.Exception).Response;
      } else if (context.Exception is NotImplementedException) {
        context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotImplemented, context.Exception);
      } else {
        context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception);
      }

      log.Error(JsonConvert.SerializeObject(new {
        type = "ErrorResponse",
        uri = context.Request.RequestUri.ToString(),
        method = context.Request.Method.ToString(),
        status_code = context.Response.StatusCode,
        reason = context.Response.ReasonPhrase,
        error = JsonConvert.SerializeObject(context.Exception)
      }), context.Exception);

    }
  }
}