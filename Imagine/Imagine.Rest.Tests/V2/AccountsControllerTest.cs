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
  public sealed class AccountsControllerTest : IDisposable {

    #region Protected Variables

    private VoiceAccountsController controller;

    #endregion Protected Variables

    #region Setup

    [TestInitialize]
    public void Setup() {
      controller = new VoiceAccountsController();
      controller.Request = new HttpRequestMessage();
      controller.Request.SetConfiguration(new HttpConfiguration());
    }

    #endregion Setup

    public void Dispose() {
      controller.Dispose();
    }

    #region GetRoutesById

    [TestMethod]
    public void GetByIdNotFound() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        var result = controller.GetById("0987654321");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void GetByIdSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.get_account_followme();
        ProductAdminService.get_product_info();
        CustomerAdminService.get_customer_info();
        PortaModelContext.get_routes_by_id();
        var result = controller.GetById("1234567890");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    #endregion GetRoutesById

    #region Post Account
    [TestMethod]
    public void PostSuccesfully() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        ProductAdminService.get_product_info();
        AccountAdminService.add_account();
        CustomerAdminService.get_customer_info();
        PortaModelContext.get_routes_by_name();
        AccountAdminService.get_custom_fields_values();
        RoutingPlanInfoOracle.get_routes_by_name();
        PortaModelContext.get_routes_by_id();
        var result = controller.PostAccount(new AccountViewModel() { Id = "123456789", ProductCode = "ABC", CustomerId = "RVTP-100001" });
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.Created), result.StatusCode.ToString());
      }
    }

    #endregion

    #region Put Account

    [TestMethod]
    public void UpdateNotFound() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        var result = controller.PutAccount("0987654321", new AccountViewModel() { CustomerId = "RVTP-100001" });
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void UpdateBadRequestWithNullBody() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        var result = controller.PutAccount("0987654321", null);
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.BadRequest), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void UpdateBadRequestWithEmptyBody() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.update_account();
        ProductAdminService.get_product_info();
        CustomerAdminService.get_customer_info();
        PortaModelContext.get_routes_by_name();
        var result = controller.PutAccount("1234567890", new AccountViewModel());
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.BadRequest), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void UpdateSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.get_account_followme();
        AccountAdminService.update_account();
        AccountAdminService.get_custom_fields_values();
        ProductAdminService.get_product_info();
        CustomerAdminService.get_customer_info();
       
        PortaModelContext.get_routes_by_name();
        PortaModelContext.get_routes_by_id();
        var result = controller.PutAccount("1234567890", new AccountViewModel() { Id = "1234567890", Suspended = false, CustomerId = "RVTP-100001" });
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void MoveAccountSuccesfully() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.move_account();
        CustomerAdminService.get_customer_info();
        var result = controller.PutAccountMove("1234567890", "RVTP-100001");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }


    #endregion Put

    #region Get VoiceMail
    [TestMethod]
    public void CreateVoiceMailSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.update_account();
        VoicemailService.get_vm_settings();
        VoicemailService.set_vm_settings();
        CustomerAdminService.get_customer_info();
        var result = controller.GetVoicemailById("1234567890");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }
    #endregion

    #region Put VoiceMail

    [TestMethod]
    public void UpdateVoiceMailNotFound() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        var result = controller.PutAccountVoiceMail("0987654321", new VoiceMailViewModel());
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.NotFound), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void UpdateVoiceMailBadRequestWithNullBody() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        var result = controller.PutAccountVoiceMail("0987654321", null);
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.BadRequest), result.StatusCode.ToString());
      }
    }

    [TestMethod]
    public void UpdateVoiceMailSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.update_account();
        VoicemailService.get_vm_settings();
        VoicemailService.set_vm_settings();
        var result = controller.PutAccountVoiceMail("1234567890", new VoiceMailViewModel() { Pin = "1234" });
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    #endregion

    #region Get Aliases
    [TestMethod]
    public void GetAliasSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.get_alias_list();
        var result = controller.GetAliasesById("1234567890");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }
    #endregion

    #region FollowMeNumbers
    [TestMethod]

    public void GetFollowMeNumberSuccess() {
      using (ShimsContext.Create()) {
        SessionAdminService.login();
        AccountAdminService.get_account_info();
        AccountAdminService.get_account_followme();
        VoicemailService.get_vm_settings();
        VoicemailService.set_vm_settings();
        CustomerAdminService.get_customer_info();
        var result = controller.GetFollowMeNumbers("1234567890");
        Assert.IsTrue(result.StatusCode.Equals(HttpStatusCode.OK), result.StatusCode.ToString());
      }
    }

    #endregion

  }
}