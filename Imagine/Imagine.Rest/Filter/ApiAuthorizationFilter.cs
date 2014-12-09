using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Imagine.Rest.Filter {

  /// <summary>
  /// Filter for Checking Api Authorization
  /// </summary>
  public class ApiAuthorizationFilter : AuthorizationFilterAttribute {

    /// <summary>
    /// Event which is fired when Authorization is attempted
    /// </summary>
    /// <param name="actionContext">Context for the Http Action</param>
    public override void OnAuthorization(HttpActionContext actionContext) {
      if (!actionContext.Request.Headers.Contains("SYSTEM_TOKEN")) {
        throw new HttpResponseException(HttpStatusCode.Unauthorized);
      }
    }
  }
}