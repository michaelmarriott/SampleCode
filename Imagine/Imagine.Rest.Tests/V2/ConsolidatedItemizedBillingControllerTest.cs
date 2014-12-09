using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.Controller.V2;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class ConsolidatedItemizedBillingControllerTest {


    ConsolidatedItemizedBillingController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new ConsolidatedItemizedBillingController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetConsolidatedItemizedBilling() {
      using (ShimsContext.Create()) {

      }
     
    }

  }
}
