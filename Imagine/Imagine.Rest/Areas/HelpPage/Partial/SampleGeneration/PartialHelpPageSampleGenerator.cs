using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Xml.XPath;

namespace Imagine.Rest.Areas.HelpPage {
  public partial class HelpPageSampleGenerator {

    /// <summary>
    /// Gets the response body samples for a given <see cref="ApiDescription"/>.
    /// </summary>
    /// <param name="api">The <see cref="ApiDescription"/>.</param>
    /// <returns>The samples keyed by media type.</returns>
    public object GetSampleResponseAttribute(ApiDescription api) {
      return GetSampleResponseAttribute(api, SampleDirection.Response);
    }

    public object GetSampleResponseAttribute(ApiDescription api, SampleDirection sampleDirection) {
      if (api == null) {
        throw new ArgumentNullException("api");
      }
      string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
      string actionName = api.ActionDescriptor.ActionName;
      IEnumerable<string> parameterNames = api.ParameterDescriptions.Select(p => p.Name);
      Collection<MediaTypeFormatter> formatters;
      Type type = ResolveType(api, controllerName, actionName, parameterNames, sampleDirection, out formatters);
      if (type != null && !typeof(HttpResponseMessage).IsAssignableFrom(type)) {
        if (type.IsArray) {
          return type.GetElementType().GetProperties();
        } else {
          return type.GetProperties();
        }
      }
      return null;
    }

  }
}