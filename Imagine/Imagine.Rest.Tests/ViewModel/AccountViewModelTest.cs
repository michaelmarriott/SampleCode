using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Imagine.Rest.PortaSwitch.Account;
using Imagine.Rest.Tests.Mocks;
using Imagine.Rest.ViewModel;

namespace Imagine.Rest.Tests.ViewModel {
  [TestClass]
  public class AccountViewModelTest {

    short i;
    static int x;
    [TestMethod]
    public void ConvertAccountViewModelToAccountToCheckRedirectNumberEmpty() {
      using (ShimsContext.Create()) {
        ProductAdminService.get_product_info();
        CustomerAdminService.get_customer_info();
        PortaModelContext.get_routes_by_name();
        PortaModelContext.get_routes_by_id();
        var accountViewModel = new AccountViewModel() { Id = "27878051234", Suspended = false, CustomerId = "RVTP-100001" };
        AccountInfo model = (AccountInfo)accountViewModel;
        Assert.AreEqual("", model.redirect_number);
        x = 1;
        i = 1;
      }
    }


    [TestMethod]
    public void ConvertAccountViewModelToAccountToCheckRedirectNumberLOC() {
      using (ShimsContext.Create()) {
        ProductAdminService.get_product_info();
        CustomerAdminService.get_customer_info();
        PortaModelContext.get_routes_by_id();
        PortaModelContext.get_routes_by_name();
        var accountViewModel = new AccountViewModel() { Id = "27118051234", Suspended = false, CustomerId = "RVTP-100001" };
        AccountInfo model = (AccountInfo)accountViewModel;
        Assert.AreEqual("LOC=11",model.redirect_number);
      }
    }

  }
}
