using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Imagine.Rest.Helper.Authentication;
using Imagine.Rest.Model;

namespace Imagine.Rest.PortaSwitch.Product {

  /// <summary>
  /// Model for the Product that connects to the WebService
  /// </summary>
  public partial class ProductInfo : WebServiceModel<ProductInfo> {
    #region Private Variables
    private static readonly ILog log = LogManager.GetLogger(typeof(ProductInfo));
    private AuthInfoStructure authInfo;
    private static Dictionary<int, ProductInfo> productCache = new Dictionary<int, ProductInfo>();
    #endregion

    public ProductInfo() {
      CreateAuthInfo();
    }

    /// <summary>
    /// Finds the Product with the given identifier
    /// </summary>
    /// <param name="id">Identifier used for the Product</param>
    /// <returns>ProductInfo object if found otherwise null</returns>
    public ProductInfo Find(int id) {
      ProductInfo product = GetProductInfo(new GetProductInfoRequest() { i_product = id, i_productSpecified = true });
      return product;
    }

    private ProductInfo GetProductInfo(GetProductInfoRequest request) {
      Stopwatch benchmark = new Stopwatch();
      benchmark.Start();
      ProductInfo product = null;
      if (request.i_productSpecified && productCache.ContainsKey((int)request.i_product)) {
        product = productCache[(int)request.i_product];
      }
      else {
        using (var service = new ProductAdminService()) {
          service.AuthInfoStructureValue = authInfo;
          var result = service.get_product_info(request);
          product = result.product_info;
          if (product.i_product != null) {
            if (!productCache.ContainsKey((int)product.i_product)) { productCache.Add((int)product.i_product, product); }
          }
        }
      }
      benchmark.Stop();
      log.Debug(JsonConvert.SerializeObject(new { operation = "get_product_info", execution_time_ms = benchmark.ElapsedMilliseconds }));
      return product;
    }

    /// <summary>
    /// Finds the Product with the given name
    /// </summary>
    /// <param name="name">Name of the Product (Product Code)</param>
    /// <returns>A ProductInfo Model if it exists</returns>
    public ProductInfo Find(string name) {
      ProductInfo product = GetProductInfo(new GetProductInfoRequest() { name = name });
      return product;
    }

    public bool Save() { throw new NotImplementedException(); }

    public bool Create() {  throw new NotImplementedException(); }

    public bool Delete() { throw new NotImplementedException(); }

    private void CreateAuthInfo() {
      var authentication = new ServiceAuthentication();
      authInfo = new AuthInfoStructure() {
        login = authentication.UserName,
        password = authentication.Password
      };
    }
  }
}