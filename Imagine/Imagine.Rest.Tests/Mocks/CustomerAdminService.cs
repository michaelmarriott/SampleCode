using Imagine.Rest.PortaSwitch.Customer;
using Imagine.Rest.PortaSwitch.Customer.Fakes;

namespace Imagine.Rest.Tests.Mocks {

  public class CustomerAdminService {

    public static CustomerInfo ValidCustomer() {
      return new CustomerInfo() {
        i_customer = 3,
        bill_status = "O",
        i_customer_type = 1,
        i_billing_period = 4,
        i_parent = 1
      };
    }

    public static void get_customer_list() {
      ShimCustomerAdminService.AllInstances.get_customer_listGetCustomerListRequest = (c, request) => {
        switch (request.i_parent) {
          case 1:
            return new GetCustomerListResponse() { customer_list = new CustomerInfo[] { ValidCustomer() } };
          case 3:
            return new GetCustomerListResponse() { customer_list = new CustomerInfo[] { ValidCustomer() } };
        }
        return null;
      };
    }

    public static void get_customer_info() {
      ShimCustomerAdminService.AllInstances.get_customer_infoGetCustomerInfoRequest = (c, request) => {
        switch (request.i_customer) {
          case 1:
            return new GetCustomerInfoResponse() { customer_info = new CustomerInfo() { i_customer = 1, name = "RVTP" } };
          case 3:
            return new GetCustomerInfoResponse() { customer_info = new CustomerInfo() { i_customer = 3, name = "RVTP" } };
        }
        switch (request.name) {
          case "RVTP-100000":
            return null;
          case "RVTP-100001":
            return new GetCustomerInfoResponse() { customer_info = ValidCustomer() };
          case "RVTP-100002":
            return new GetCustomerInfoResponse() { customer_info = new CustomerInfo() { i_customer = 2, i_parent = 1} };
          case "RVTP":
            return new GetCustomerInfoResponse() { customer_info = new CustomerInfo() { i_customer = 1, i_parent = 1, name = "RVTP" } };
          case "RVTP-Teleprenuer":
            return new GetCustomerInfoResponse() { customer_info = new CustomerInfo() { i_customer = 1, i_parent = 1, name = "RVTP-Teleprenuer" } };
        }
        return null;
      };
    }

    public static void add_customer() {
      ShimCustomerAdminService.AllInstances.add_customerAddCustomerRequest = (c, request) => {
        switch (request.customer_info.name) {
          case "RVTP-NEW":
            return new AddUpdateCustomerResponse() { i_customer = 4 };
        }
        return null;
      };
    }


  }
}