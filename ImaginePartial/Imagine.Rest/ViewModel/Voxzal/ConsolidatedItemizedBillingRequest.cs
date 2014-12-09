using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imagine.Rest.ViewModel.Voxzal {

  /// <summary>
  /// A Request Object For Consolidated Itemized Billing
  /// </summary>
  public class ConsolidatedItemizedBillingRequest {

    public int BillRunId { get; set; }
    public string CustomerId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string EmailAddress { get; set; }

  }
}