using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryQ;
using Vox.Porta.Rest.Tests.Mocks;
using Imagine.Rest.Controller.V2;

namespace Vox.Porta.Rest.Tests.V2 {

  [TestClass]
  public class BillRunInstructionControllerTest {

    private BillRunController controller;

    [TestInitialize]
    public void before_each() {
      controller = new BillRunController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetAllBillRunInstructions() {
      using (ShimsContext.Create()) {
        var response = controller.GetAll(0, "201402", 20, 0);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

  }
}