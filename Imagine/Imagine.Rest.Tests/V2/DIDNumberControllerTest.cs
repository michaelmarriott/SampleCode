using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Tests.Mocks;
using Imagine.Rest.ViewModel;

namespace Imagine.Rest.Tests.V2 {

  [TestClass]
  public sealed class DIDNumberControllerTest : IDisposable {

    #region Private Variables

    private VoiceDIDNumbersController controller;

    #endregion Private Variables

    #region Setup

    [TestInitialize]
    public void Setup() {
      controller = new VoiceDIDNumbersController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    #endregion Setup

    public void Dispose() {
      controller.Dispose();
    }

    public void ReserveNumberSuccesfully() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        PortaModelContext.get_did_list();
        PortaModelContext.get_vendor_batch();
        DIDAdminService.release_number();
        DIDAdminService.reserve_number();
        var result = controller.PutReserve(new DIDNumberReserveViewModel { ResellerIdentifier = "VATL", Amount = 2, IsBlockSet = true });
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

  }
}
