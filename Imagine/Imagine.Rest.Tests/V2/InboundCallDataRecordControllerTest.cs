using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Tests.Mocks;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class InboundCallDataRecordControllerTest {

    InboundCallDataRecordController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new InboundCallDataRecordController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetInboundCallDataRecord() {
      int lobId = 1;
      string dateStart = "20140101";
      string dateEnd = "20140101";
      string customerId = "";
      string accountCode = "SBS02";
      string deviceIdList = "";
      int offset = 0;
      int limit = 0;
      using (ShimsContext.Create()) {
        CallDataRecord.PopulateInboundCallDataRecordDataSet();
        var response = controller.GetBy(lobId, dateStart, dateEnd, customerId, accountCode, deviceIdList, offset, limit);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
     
    }

  }
}
