using System;
using Imagine.Rest.PortaSwitch.Account;
using Imagine.Rest.PortaSwitch.Account.Fakes;

namespace Imagine.Rest.Tests.Mocks {

  public class AccountAdminService {

    public static AccountInfo ValidAccount() {
      return new AccountInfo() {
        i_account = 1,
        i_accountSpecified = true,
        follow_me_enabled = "F",
        um_enabled = "F",
        activation_date = DateTime.Now,
        activation_dateSpecified = true,
        i_customer = 3
      };
    }

    public static void add_account() {
      ShimAccountAdminService.AllInstances.add_accountAddAccountRequest = (c, request) => {
        if (String.IsNullOrEmpty(request.account_info.id)) {
          return new AddUpdateAccountResponse() { i_account = 0 };
        }
        else {
          return new AddUpdateAccountResponse() { i_account = 1 };
        }
      };
    }

    public static void move_account() {
      ShimAccountAdminService.AllInstances.move_accountMoveAccountRequest = (c, request) => {
        if (request.i_account == 0) {
          return new MoveAccountResponse() { i_account = 0, old_i_account = 0 };
        }
        else {
          return new MoveAccountResponse() { i_account = 999, old_i_account = request.i_account };
        }
      };
    }

    public static void update_account() {
      ShimAccountAdminService.AllInstances.update_accountUpdateAccountRequest = (c, request) => {
        if (String.IsNullOrEmpty(request.account_info.id)) {
          return new AddUpdateAccountResponse() { i_account = 0 };
        }
        else {
          return new AddUpdateAccountResponse() { i_account = 1 };
        }
      };
    }

    public static void get_custom_fields_values() {
      ShimAccountAdminService.AllInstances.get_custom_fields_valuesGetAccountCustomFieldsValuesRequest = (c, request) => {
        return new GetAccountCustomFieldsValuesResponse();
      };
    }

    public static void get_account_list() {
      ShimAccountAdminService.AllInstances.get_account_listGetAccountListRequest = (c, request) => {
        switch (request.i_customer) {
          case 1:
            return new GetAccountListResponse() { account_list = new AccountInfo[] { ValidAccount() } };

          case 2:
            return new GetAccountListResponse();

          case 3:
            return new GetAccountListResponse() { account_list = new AccountInfo[] { ValidAccount() } };
        }
        return null;
      };
    }

    public static void get_account_info() {
      ShimAccountAdminService.AllInstances.get_account_infoGetAccountInfoRequest = (c, request) => {
        switch (request.id) {
          case "1234567890":
            return new GetAccountInfoResponse() { account_info = ValidAccount() };
        }
        return null;
      };
    }

    public static void get_account_followme() {
      ShimAccountAdminService.AllInstances.get_account_followmeGetAccountFollowMeRequest = (c, request) => {
        switch (request.i_account) {
          case 1:
            return new GetAccountFollowMeResponse() { followme_numbers = new FollowMeNumberInfo[] { new FollowMeNumberInfo { i_follow_me_number = 1, name = "271234567" } } };
        }
        return null;
      };
    }

    public static void get_alias_list() {
      ShimAccountAdminService.AllInstances.get_alias_listGetAccountAliasListRequest = (c, request) => {
        switch (request.i_master_account) {
          case 1:
            return new GetAccountAliasListResponse() { alias_list = new AliasInfo[] { new AliasInfo() { i_account = 1 } } };
        }
        return null;
      };
    }
  }
}