using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Imagine.Rest.Data;

namespace Imagine.Rest.ViewModel.Dr {

  [JsonObject]
  public class CloudViewModel {

    [JsonProperty]
    public int CloudId { get; set; }
    public string Name { get; set; }
    public int? NetworkId { get; set; }
    public int TimeZoneMin { get; set; }

    public static explicit operator CLOUD(CloudViewModel viewModel) {
      return new CLOUD() {
        CLOUDID = viewModel.CloudId,
        NAME = viewModel.Name,
        NETWORKID = viewModel.NetworkId,
        TIMEZONEMIN = viewModel.TimeZoneMin
      };
    }
    public static explicit operator CloudViewModel(CLOUD model) {
      return new CloudViewModel() {
        CloudId = (int)model.CLOUDID,
        Name = model.NAME,
        NetworkId = (int)model.NETWORKID,
        TimeZoneMin = (int)model.TIMEZONEMIN
      };
    }

    public override string ToString() {
      return JsonConvert.SerializeObject(this);
    }

  }
}