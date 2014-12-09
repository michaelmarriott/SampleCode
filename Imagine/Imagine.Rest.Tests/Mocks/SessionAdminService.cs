using System;
using Imagine.Rest.PortaSwitch.Authentication.Fakes;

namespace Imagine.Rest.Tests.Mocks {

  public class SessionAdminService {

    public static void login() {
      ShimSessionAdminService.AllInstances.loginStringString = (c, username, password) => {
        return Guid.NewGuid().ToString();
      };
    }
  }
}