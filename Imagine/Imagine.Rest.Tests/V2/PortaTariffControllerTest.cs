using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.Controller.V2;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class PortaTariffControllerTest {

    PortaTariffController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new PortaTariffController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }


    [TestMethod]
    public void TestMethod1() {
    }
  }
}
