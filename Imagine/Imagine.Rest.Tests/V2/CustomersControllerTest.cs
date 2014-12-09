using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Imagine.Rest.Controller.V2;
using Imagine.Rest.Tests.Mocks;
using Imagine.Rest.ViewModel;

namespace Imagine.Rest.Tests.V2 {

  [TestClass]
  public sealed class CustomersControllerTest : IDisposable {

    #region Private Variables

    private VoiceCustomersController controller;

    #endregion Private Variables

    #region Setup

    [TestInitialize]
    public void Setup() {
      controller = new VoiceCustomersController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    #endregion Setup

    public void Dispose() {
      controller.Dispose();
    }

    #region Get

    [TestMethod]
    public void GetCustomersNoContent() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        CustomerAdminService.get_customer_list();
        var result = controller.GetAll(0, 100, null);
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void GetCustomersBadRequest() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        CustomerAdminService.get_customer_list();
        var result = controller.GetAll(0, 100, "INVALID");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.BadRequest), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void GetCustomersSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        CustomerAdminService.get_customer_list();
        var result = controller.GetAll(0, 100, "RVTP");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    #endregion Get

    #region GetRoutesByName

    [TestMethod]
    public void GetByNameNotFound() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        var result = controller.GetByName("RVTP-NOTFOUND");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void GetByNameSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        var result = controller.GetByName("RVTP-100001");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    #endregion GetRoutesByName

    #region GetAccountsByCustomerName

    [TestMethod]
    public void GetAccountsByCustomerNameNotFound() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        var result = controller.GetAccountsByCustomerName("RVTP-100000");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void GetAccountsByCustomerNameSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        AccountAdminService.get_account_list();
        AccountAdminService.get_account_followme();
        ProductAdminService.get_product_info();
        RoutingPlanInfoOracle.get_routes_by_id();
        var result = controller.GetAccountsByCustomerName("RVTP-100001");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void GetAccountsByCustomerNameNoContent() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();
        AccountAdminService.get_account_list();
        var result = controller.GetAccountsByCustomerName("RVTP-100002");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NoContent), result.StatusCode.ToString());
      }
    }

    #endregion GetAccountsByCustomerName


    #region Post
    [TestMethod]
    public void PostNewCustomerSuccesful() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        CustomerAdminService.get_customer_info();

        CustomerAdminService.add_customer();
        var result = controller.PostCustomer(new CustomerViewModel() { ResellerIdentifier = "RVTP", Id = "RVTP-NEW" });
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.Created), result.StatusCode.ToString());
      }
    }

    #endregion


  }
}