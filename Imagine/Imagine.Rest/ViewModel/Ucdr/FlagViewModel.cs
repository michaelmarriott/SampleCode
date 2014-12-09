using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Imagine.Rest.Data;

namespace Imagine.Rest.ViewModel.Dr {
   [JsonObject]
  public class FlagViewModel {

    public int FlagId { get; set; }
    public int? CloudId { get; set; }
    public string Name { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? Status { get; set; }

    public static explicit operator FLAG(FlagViewModel viewModel) {
      return new FLAG() {
        FLAGID = viewModel.FlagId,
        CLOUDID = viewModel.CloudId,
        NAME = viewModel.Name,
        DATEFROM = viewModel.DateFrom,
        DATETO = viewModel.DateTo,
        STATUS = viewModel.Status
      };
    }
    public static explicit operator FlagViewModel(FLAG model) {
      return new FlagViewModel() {
        FlagId = (int)model.FLAGID,
        CloudId = (int)model.CLOUDID,
        Name = model.NAME,
        DateFrom = model.DATEFROM,
        DateTo = model.DATETO,
        Status = (int)model.STATUS
      };
    }

    public override string ToString() {
      return JsonConvert.SerializeObject(this);
    }

  }
}