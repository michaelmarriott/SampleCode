using System;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Imagine.Rest.Filter {

  /// <summary>
  /// Handles logging operations for the requests
  /// </summary>
  public class LogFilter: ActionFilterAttribute {

    #region Private Variables
    private Stopwatch benchMark;

    /// <summary> Logger Instance </summary>
    private static readonly ILog log = LogManager.GetLogger(typeof(LogFilter));

    private string id;
    #endregion

    public override void OnActionExecuting(HttpActionContext actionContext) {
      benchMark = new Stopwatch();
      benchMark.Start();
      id = Guid.NewGuid().ToString();
      var args = new StringBuilder();
      foreach (var value in actionContext.ActionArguments) {
        args.AppendLine(value.Key + " = " + value.Value);
      }
      log.Info(JsonConvert.SerializeObject(new {
        type = "Request",
        id = id,
        uri = actionContext.Request.RequestUri.ToString(),
        method = actionContext.Request.Method.ToString(),
        referrer = actionContext.Request.Headers.Referrer,
        args = args.ToString()
      }));
    }

    /// <summary>
    /// Event Handler for when the Action is Executed
    /// </summary>
    /// <param name="actionExecutedContext">Http Context for the Action</param>
    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
      benchMark.Stop();
      if (actionExecutedContext.Response != null) {
        log.Info(JsonConvert.SerializeObject(new {
          type = "Response",
          id = id,
          uri = actionExecutedContext.Request.RequestUri.ToString(),
          method = actionExecutedContext.Request.Method.ToString(),
          status_code = actionExecutedContext.Response.StatusCode,
          execution_time_ms = benchMark.ElapsedMilliseconds
        }));
      }
    }
  }
}