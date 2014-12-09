using log4net;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Imagine.Rest.PortaSwitch.Account;
using Imagine.Rest.PortaSwitch.Customer;
using Imagine.Rest.ViewModel;

namespace Imagine.Rest.Controller.V2 {

  /// <summary> Contains the methods that can be performed on the customers resource </summary>
  [RoutePrefix("2/Voice/VoiceCustomers")]
  public class VoiceCustomersController : ApiController {
    private static readonly ILog log = LogManager.GetLogger(typeof(VoiceCustomersController));

    #region Get

    /// <summary>    
    /// Retrieves all of the customers for the given reseller and search filter &lt;br/&gt;
    /// Replaces: &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/CustomersForReseller" &gt;VOX/VOICE/REQUEST/RESELLER&lt;/a&gt;  
    /// </summary>
    /// <param name="resellerId">Reseller name that the search will be restricted to [optional]</param>
    /// <param name="name">Customer name, use of wildcards % to search for customers [optional]</param>
    /// <param name="limit">Limit the number of customers to return (Default = 10)</param>
    /// <param name="offset">Offset from the 0 the customers to return (Default = 0)</param>
    /// <returns>A Status Code 200 and a Array of CustomerViewModel Results, 404 is returned if no results found</returns>
    [Route("")]
    [ResponseType(typeof(CustomerViewModel[]))]
    public HttpResponseMessage GetAll(int limit = 10, int offset = 0, string resellerId = null, string name = null) {
      HttpResponseMessage response = null;
      int parentCustomerId = 0;
      if (!String.IsNullOrEmpty(resellerId)) {
        var reseller = new CustomerInfo().Find(resellerId);
        if (reseller == null) { return Request.CreateResponse(HttpStatusCode.BadRequest); }
        parentCustomerId = reseller.i_customer;
      }
      CustomerInfo[] customers = new CustomerInfo().FindAll(parentCustomerId, name, limit, offset);
      if (customers.Length == 0) {
        response = Request.CreateResponse(HttpStatusCode.NotFound);
      }
      else {
        response = Request.CreateResponse(HttpStatusCode.OK, customers.ToList().ConvertAll(e => (CustomerViewModel)e));
      }
      string json = JsonConvert.SerializeObject(response.Content);
      return response;
    }

    /// <summary>    
    /// Retrieves the customer for the given identifier
    /// </summary>
    /// <param name="id">Identifier of the customer</param>
    /// <returns>The Customer if it exists, otherwise a 404 (Not Found) if the customer does not exist</returns>
    [Route("{id}")]
    [ResponseType(typeof(CustomerViewModel))]
    public HttpResponseMessage GetByName(string id) {
      CustomerInfo customer = new CustomerInfo().Find(id);
      if (customer == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
      return Request.CreateResponse(HttpStatusCode.OK, (CustomerViewModel)customer);
    }

    /// <summary>    
    /// Retrieves all of the accounts for the given customer &lt;/br&gt;
    /// Replaces: &lt;a href="http://wiki.voxtelecom.co.za:8080/display/VoxDevOps/Imagine.Request.Customer" &gt;VOX/VOICE/REQUEST/CUSTOMER&lt;/a&gt; 
    /// </summary>
    /// <param name="id">The Identifier for the customer (Provided as the customer name)</param>
    /// <param name="limit">Limit the number of customers to return (Default = 10)</param>
    /// <param name="offset">Offset from the 0 the customers to return (Default = 0)</param>
    /// <returns>If accounts exist it will return the list of accounts for the customers with a status code 200 (OK), 
    /// otherwise it will return a 204 (NoContent) if the customer has no accounts.</returns>
    [Route("{id}/VoiceAccounts")]
    [ResponseType(typeof(AccountViewModel[]))]
    public HttpResponseMessage GetAccountsByCustomerName(string id, int limit = 10, int offset = 0) {
      CustomerInfo customerInfo = new CustomerInfo().Find(id);
      if (customerInfo == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
      AccountInfo[] accounts = customerInfo.GetAccounts(limit, offset);
      if (accounts.Length == 0) { return Request.CreateResponse(HttpStatusCode.NoContent); }
      return Request.CreateResponse(HttpStatusCode.OK, accounts.ToList().ConvertAll(e => (AccountViewModel)e)); 
    }

    #endregion Get Methods

    #region Post

    /// <summary> Adds a new Voice Customer. This is part of the provisioning process </summary>
    /// <param name="customerViewModel">Customer Model</param>
    /// <returns>Status Code 200 if the account is succesfully updated, or a 400 if the information is incorrect</returns>
    [Route("")]
    [ResponseType(typeof(HttpResponseMessage))]
    public HttpResponseMessage PostCustomer(CustomerViewModel customerViewModel) {
      log.Info("POSTING");
      log.Info(JsonConvert.SerializeObject(customerViewModel));
      CustomerInfo customerInfo = (CustomerInfo)customerViewModel;
      customerInfo.Create();
      return Request.CreateResponse(HttpStatusCode.Created, new { Id = customerViewModel.Id });
    }

    #endregion Post Method

    #region Put

    /// <summary> Updates an existing Voice Customer.</summary>
    /// <param name="id">The Identifier for the customer (Provided as the customer name)</param>
    /// <param name="customerViewModel">Customer Model</param>
    /// <returns>Status Code 200 if the account is succesfully updated, or a 400 if the information is incorrect</returns>
    [Route("{id}")]
    [ResponseType(typeof(HttpResponseMessage))]
    public HttpResponseMessage PutCustomer(string id, CustomerViewModel customerViewModel) {
      log.Info("PUTTING");
      log.Info(JsonConvert.SerializeObject(customerViewModel));
      var customer = new CustomerInfo().Find(id);
      customerViewModel.InternalId = customer.i_customer;
      customerViewModel.Id = customer.name;
      var customerInfo = (CustomerInfo)customerViewModel;
      customerInfo.Save();
      var customerResult = new CustomerInfo().Find(id);
      return Request.CreateResponse(HttpStatusCode.OK, (CustomerViewModel)customerResult);
    }

    #endregion Put Method
  }
}