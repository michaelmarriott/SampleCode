using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Model.Dr.Fakes;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class TariffControllerTest {

    TariffController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new TariffController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetTariffSummaryBadRequest() {
      string callplanId = "";
      string zones = "1";
      using (ShimsContext.Create()) {
        ShimTariffSummary.AllInstances.FillDataSetDataSetOracleCommand = (r, d, o) => {
          d = new DataSet() { };
        };
        var response = controller.GetBy(callplanId, zones);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

  }
}
