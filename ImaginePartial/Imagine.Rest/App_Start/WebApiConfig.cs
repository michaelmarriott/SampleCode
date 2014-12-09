using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting;

namespace Imagine.Rest {

  public static class WebApiConfig {

    public static void Register(HttpConfiguration config) {
      config.MapHttpAttributeRoutes();

      config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
      config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));
      config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      config.Formatters.Add(new ProtoBufFormatter());


    }
  }
}