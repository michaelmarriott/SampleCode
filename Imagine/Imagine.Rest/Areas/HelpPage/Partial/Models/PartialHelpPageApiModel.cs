using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Web.Http.Description;

namespace Imagine.Rest.Areas.HelpPage.Models {

  /// <summary>
  /// The model that represents an API displayed on the help page.
  /// </summary>
  public partial class HelpPageApiModel {

    public object SampleResponseAttribute { get; set; }

  }
}
