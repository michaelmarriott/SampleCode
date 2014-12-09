using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Tests.Mocks;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class OutboundCallDataRecordSummaryControllerTest {

    OutboundCallDataRecordSummaryController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new OutboundCallDataRecordSummaryController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetIOutboundCallDataRecordSummary() {
      int lobId = 1;
      string dateStart = "20140101";
      string dateEnd = "20140101";
      string customerId = "";
      string accountCode = "SBS02";
      string deviceIdList = "";
      int offset = 0;
      int limit = 0;
      using (ShimsContext.Create()) {
        CallDataRecord.PopulateOutboundCallDataRecordDataSet();
        var response = controller.GetBy(lobId, dateStart, dateEnd, customerId, accountCode, deviceIdList, offset, limit);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

  }
}
