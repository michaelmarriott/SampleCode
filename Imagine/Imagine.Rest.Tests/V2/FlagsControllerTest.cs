using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Data.Fakes;
using System.Collections.Generic;
using Imagine.Rest.Data;
using Imagine.Rest.ViewModel.Dr;
using Vox.Porta.Rest.Tests.Mocks;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class FlagsControllerTest {

    FlagsController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new FlagsController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetFlags() {
      string name = "Rating";
      int limit = 20;
      int offset = 0;
      string orderby = "flagid";
      using (ShimsContext.Create()) {
        FLAGContext.find_by_name();
        var response = controller.Get(name, limit, offset, orderby);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void CreateFlag() {
      using (ShimsContext.Create()) {
        FLAGContext.create();
        var response = controller.Post(new FlagViewModel() { Name = "Rating", CloudId = 1, DateFrom = DateTime.Now, DateTo = DateTime.Now, FlagId = 1, Status = 0 });
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }


    [TestMethod]
    public void SaveFlag() {
      using (ShimsContext.Create()) {
        FLAGContext.save();
        var response = controller.Put(1, new FlagViewModel() { Name = "Rating", CloudId = 1, DateFrom = DateTime.Now, DateTo = DateTime.Now, FlagId = 1, Status = 0 });
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }

    }


    [TestMethod]
    public void DeleteFlag() {
      using (ShimsContext.Create()) {
        FLAGContext.find();
        FLAGContext.delete();
        var response = controller.Delete(1);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

  }
}
