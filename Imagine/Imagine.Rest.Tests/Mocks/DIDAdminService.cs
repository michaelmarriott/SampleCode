using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagine.Rest.PortaSwitch.DID;
using Imagine.Rest.PortaSwitch.DID.Fakes;

namespace Imagine.Rest.Tests.Mocks {
  public class DIDAdminService {
    public static void reserve_number() {
      ShimDIDAdminService.AllInstances.reserve_numberReserveDIDNumberRequest = (c, request) => {
        return new ReserveDIDNumberResponse() { success = 1 };
      };
    }

    public static void release_number() {
      ShimDIDAdminService.AllInstances.release_numberReleaseDIDNumberRequest = (c, request) => {
        return new ReleaseDIDNumberResponse() { success = 1 };
      };
    }


  }
}
