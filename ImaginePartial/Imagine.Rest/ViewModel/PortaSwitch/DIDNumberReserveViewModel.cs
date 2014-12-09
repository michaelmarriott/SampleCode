using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Imagine.Rest.ViewModel {
  /// <summary> Did Number Reservation </summary>
  public class DIDNumberReserveViewModel {

    /// <summary> Reseller Identifier  </summary>
      [Required]
      public String ResellerIdentifier {get;set;}

    /// <summary> Amount of Numbers to Reserve </summary>
      [DefaultValue (1)]
      public int Amount { get; set; }

       /// <summary> Only retrieve amount of numbers sequentially</summary>
      [DefaultValue (false)]
      public bool IsBlockSet { get; set; }

      /// <summary> Prefix of numbers to reserve </summary>
      public string Prefix { get; set; }

      /// <summary> Test Model Generator for Documentation </summary>
      /// <returns>A Test Object of DIDNumberReserveViewModel</returns>
      static public DIDNumberReserveViewModel TestModel() {
        var model = new DIDNumberReserveViewModel() {
          ResellerIdentifier = "RVAT-VoxAtlantic",
          Amount = 1,
          IsBlockSet = false,
          Prefix = "2787"
        };
        return model;
      }
  }
}