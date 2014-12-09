using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Data;
using Imagine.Rest.Tests.Mocks;
using Imagine.Rest.ViewModel.Dr;

namespace Imagine.Rest.Tests.V2 {
  [TestClass]
  public class CloudControllerTest {

    CloudController controller;
    [TestInitialize]
    public void SetUp() {
      controller = new CloudController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    [TestMethod]
    public void GetCloud() {
      string name = "Rating";
      int limit = 20;
      int offset = 0;
      string orderby = "cloudid";


      var data = new List<CLOUD> {
        new CLOUD { NAME = "Rating", CLOUDID=1, NETWORKID=1, TIMEZONEMIN=27 },
        new CLOUD {  NAME = "Rating", CLOUDID=2, NETWORKID=1, TIMEZONEMIN=27 },
        new CLOUD {  NAME = "Rating", CLOUDID=4, NETWORKID=1, TIMEZONEMIN=27 }
      }.AsQueryable();


     var mockSet = Substitute.For<IQueryable<CLOUD>>();

      // And then as you do:
  //   

         


         mockSet.Provider.Returns(data.Provider);
         mockSet.Expression.Returns(data.Expression);
         mockSet.ElementType.Returns(data.ElementType);
         mockSet.GetEnumerator().Returns(data.GetEnumerator());

         ((IQueryable<CLOUD>)mockSet).Provider.Returns(data.Provider);
      

         var mockContext = Substitute.For<DrEntity>();
//         mockContext.CLOUDs.Returns(data);

      using (ShimsContext.Create()) {
      //  CLOUDContext.find_by_name();
        var response = controller.GetAll(name, limit, offset);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void CreateCloud() {
     
      using (ShimsContext.Create()) {
         CLOUDContext.create();
         var response = controller.Post(new CloudViewModel() { Name = "Rating", CloudId = 1, NetworkId = 1, TimeZoneMin = 120 });
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }


    [TestMethod]
    public void SaveCloud() {
      using (ShimsContext.Create()) {
        CLOUDContext.save();
        var response = controller.Put(1, new CloudViewModel() { Name = "Rating", CloudId = 1, NetworkId=1, TimeZoneMin=120 });
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }

    }


    [TestMethod]
    public void DeleteCloud() {
      using (ShimsContext.Create()) {
        CLOUDContext.find_by_name();
        CLOUDContext.delete();
        var response = controller.Delete(1);
        Assert.IsTrue(response.StatusCode.Equals(HttpStatusCode.OK), response.StatusCode.ToString());
      }
    }

  }
}
