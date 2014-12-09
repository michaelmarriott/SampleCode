using Imagine.Rest.PortaSwitch.Product;
using Imagine.Rest.PortaSwitch.Product.Fakes;

namespace Imagine.Rest.Tests.Mocks {

  public class ProductAdminService {

    public static void get_product_info() {
      ShimProductAdminService.AllInstances.get_product_infoGetProductInfoRequest = (c, r) => {
        return new GetProductInfoResponse() {
          product_info = new ProductInfo() {
            name = "TestProduct",
            i_product = 1
          }
        };
      };
    }
  }
}