using System;
using System.Collections.Generic;

namespace Imagine.Rest.Model.Voxzal {

  /// <summary>
  /// Most of this was pulled from existing ConsolidatedItemizedBilling code
  /// </summary>
  public class ConsolidatedItemizedBilling {

    int Id { get; set; }
    string CustomerId { get; set; }
    string FileName { get; set; }
    DateTime DateFrom { get; set; }
    DateTime DateTo { get; set; }
    byte[] File { get; set; }
    bool CallsInAndOut { get; set; }
    string EmailAddress { get; set; }
    List<string> customers = null;


  }
}
