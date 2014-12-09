using System;
using Imagine.Rest.Model.PortaSwitch;
using Imagine.Rest.Model.PortaSwitch.Fakes;


namespace Imagine.Rest.Tests.V2 {

  public class RoutingPlanInfoOracle {
    
    public static void get_routes_by_name() {

      ShimRoutingPlanInfo.AllInstances.GetRoutesByNameString = (c, s) => {
        return new RoutingPlanInfo() {
          Id = 1,
          Name = s
        };
      };
    }

    public static void get_routes_by_id() {

      ShimRoutingPlanInfo.AllInstances.GetRoutesByIdInt32 = (c, i) => {
        return new RoutingPlanInfo() {
          Id = i,
          Name = "MyName"
        };
      };
    }


  }
}