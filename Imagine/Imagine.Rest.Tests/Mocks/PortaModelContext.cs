using System.Collections.Generic;
using Imagine.Rest.Data;
using Imagine.Rest.Model.PortaSwitch;
using Imagine.Rest.Model.PortaSwitch.Fakes;
using Imagine.Rest.PortaSwitch.DID.Fakes;

namespace Imagine.Rest.Tests.Mocks {
  class PortaModelContext {

    public static void get_vendor_batch() {
      ShimDIDNumberInfo.AllInstances.GetVendorBatchInt32StringEntities = (c, e, r, context) => {
        return new VENDORDIDBATCH() { I_DV_BATCH = 1, I_ENV = 4, NAME = "Test" };
      };
    }

    public static void get_did_list() {
      ShimDIDNumberInfo.AllInstances.GetDidListInt32DateTimeEntitiesVENDORDIDBATCHString = (request, env, dt, context, vb, pf) => {
        return new List<DIDNUMBERINVENTORY>() { new DIDNUMBERINVENTORY() { PHONENUMBER = "27878050000" }, new DIDNUMBERINVENTORY() { PHONENUMBER = "27878050001" } };
      };
    }

    public static void get_routes_by_name() {
      ShimRoutingPlanInfo.AllInstances.GetRoutesByNameString = (x, s) => {
        return new RoutingPlanInfo() { Id = 1, Name = s };
      };
    }

    public static void get_routes_by_id() {
      ShimRoutingPlanInfo.AllInstances.GetRoutesByIdInt32 = (x, i) => {
        return new RoutingPlanInfo() { Id = i, Name = "RVDP-ROUTE" };
      };
    }
  }
}
