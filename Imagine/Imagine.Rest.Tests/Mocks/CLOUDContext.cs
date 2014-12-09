using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.Rest.Data;
using Imagine.Rest.Data.Fakes;

namespace Imagine.Rest.Tests.Mocks {
  public class CLOUDContext {

    public static void find_by_name() {
      ShimCLOUD.AllInstances.FindByNameStringInt32Int32 = (n, l, o, r) => {
        return new List<CLOUD>() { new CLOUD() { NAME = "Rating", CLOUDID = 1, NETWORKID = 1, TIMEZONEMIN = 120 } };
      };
    }

    public static void find() {
      ShimCLOUD.AllInstances.FindByNameStringInt32Int32 = (n, l, o, r) => {
        return new List<CLOUD>() { new CLOUD() { NAME = "Rating", CLOUDID = 1, NETWORKID = 1, TIMEZONEMIN = 120 } };
      };
    }

    public static void save() {
      ShimCLOUD.AllInstances.Save = (r) => {
        r = new CLOUD() { NAME = "Rating", CLOUDID = 1, NETWORKID = 1, TIMEZONEMIN = 120 };
        return true;
      };
    }

    public static void create() {
      ShimCLOUD.AllInstances.Create = (r) => {
        r = new CLOUD() { NAME = "Rating", CLOUDID = 1, NETWORKID = 1, TIMEZONEMIN = 120 };
        return true;
      };
    }

    public static void delete() {
      ShimCLOUD.AllInstances.Delete = (r) => {
        r = new CLOUD() { NAME = "Rating", CLOUDID = 1, NETWORKID = 1, TIMEZONEMIN = 120 };
        return true;
      };
    }



  }
}
