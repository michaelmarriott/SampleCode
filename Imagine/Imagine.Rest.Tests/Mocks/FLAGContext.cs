using System.Collections.Generic;
using Imagine.Rest.Data;
using Imagine.Rest.Model.PortaSwitch;
using Imagine.Rest.Model.PortaSwitch.Fakes;
using Imagine.Rest.PortaSwitch.DID.Fakes;
using Imagine.Rest.Data;
using Imagine.Rest.Data.Fakes;
using System;

namespace Vox.Porta.Rest.Tests.Mocks {
  class FLAGContext {

    public static void find_by_name() {
      ShimFLAG.AllInstances.FindByNameStringInt32Int32String = (n, l, o, ob, r) => {
        return new List<FLAG>() { new FLAG() { NAME = "Rating", CLOUDID = 1, DATEFROM = DateTime.Now, DATETO = DateTime.Now, FLAGID = 1, STATUS = 0 } };
      };
    }

    public static void find() {
      ShimFLAG.AllInstances.FindInt32 = (n, l ) => {
        return new FLAG() { NAME = "Rating", CLOUDID = 1, DATEFROM = DateTime.Now, DATETO = DateTime.Now, FLAGID = 1, STATUS = 0 } ;
      };
    }


    public static void create() {
      ShimFLAG.AllInstances.Create = (r) => {
        r = new FLAG() { NAME = "Rating", CLOUDID = 1, DATEFROM = DateTime.Now, DATETO = DateTime.Now, FLAGID = 1, STATUS = 0 };
        return true;
      };
    }

    public static void save() {
      ShimFLAG.AllInstances.Save = (r) => {
        r = new FLAG() { NAME = "Rating", CLOUDID = 1, DATEFROM = DateTime.Now, DATETO = DateTime.Now, FLAGID = 1, STATUS = 0 };
        return true;
      };
    }

    public static void delete() {
      ShimFLAG.AllInstances.Delete = (r) => {
        r = new FLAG() { NAME = "Rating", CLOUDID = 1, DATEFROM = DateTime.Now, DATETO = DateTime.Now, FLAGID = 1, STATUS = 0 };
        return true;
      };
    }


  }
}
