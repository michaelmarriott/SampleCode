using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Imagine.Rest.Data;

namespace Imagine.Rest.ViewModel.Dr {
  [JsonObject]
  public class BillRunViewModel {

    public int Id { get; set; }
    public int? LobId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Nullable<System.DateTime> DateFrom { get; set; }
    public Nullable<System.DateTime> DateTo { get; set; }
    public Nullable<System.DateTime> InboundDateFrom { get; set; }
    public Nullable<System.DateTime> InboundDateTo { get; set; }
    public short? IsCompleted { get; set; }
    public string Direction { get; set; }
    public short? IsMidMonth { get; set; }
    public short? IsExported { get; set; }

    public static explicit operator BILLRUN(BillRunViewModel viewModel) {
      return new BILLRUN() {
        BILLRUNID = viewModel.Id,
        DATEFROM = viewModel.DateFrom,
        DATETO = viewModel.DateTo,
        INBOUNDDATEFROM = viewModel.InboundDateFrom,
        INBOUNDDATETO = viewModel.InboundDateTo,
        DIRECTION = viewModel.Direction,
        ISCOMPLETED = viewModel.IsCompleted,
        ISEXPORTED = viewModel.IsExported,
        ISMIDMONTH = viewModel.IsMidMonth,
        LOBID = viewModel.LobId,
        NAME = viewModel.Name,
        CODE = viewModel.Code
      };
    }

    public static explicit operator BillRunViewModel(BILLRUN model) {
      return new BillRunViewModel() {
        Id = (int)model.BILLRUNID,
        DateFrom = model.DATEFROM,
        DateTo =  model.DATETO,
        InboundDateFrom = model.INBOUNDDATEFROM,
        InboundDateTo = model.INBOUNDDATETO,
        Direction = model.DIRECTION,
        IsCompleted = model.ISCOMPLETED,
        IsExported = model.ISEXPORTED,
        IsMidMonth = model.ISMIDMONTH,
        LobId = (int)model.LOBID,
        Name = model.NAME,
        Code = model.CODE
      };
    }

    public override string ToString() {
      return JsonConvert.SerializeObject(this);
    }

  }
}
