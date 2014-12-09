using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace Imagine.Rest {

  public class Global : System.Web.HttpApplication {

    protected void Application_Start(object sender, EventArgs e) {

      GlobalConfiguration.Configuration.Filters.Add(new Filter.ApiAuthorizationFilter());
      GlobalConfiguration.Configuration.Filters.Add(new Filter.LogFilter());
      GlobalConfiguration.Configuration.Filters.Add(new Filter.ExceptionFilter());
      GlobalConfiguration.Configure(WebApiConfig.Register);
      AreaRegistration.RegisterAllAreas();

      log4net.Config.XmlConfigurator.Configure();

    }

    protected void Session_Start(object sender, EventArgs e) {
    }

    protected void Application_BeginRequest(object sender, EventArgs e) {
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e) {
    }

      protected void Application_Error(object sender, EventArgs e)
      {
          
      }

    protected void Session_End(object sender, EventArgs e) {
    }

    protected void Application_End(object sender, EventArgs e) {
    }
  }
}