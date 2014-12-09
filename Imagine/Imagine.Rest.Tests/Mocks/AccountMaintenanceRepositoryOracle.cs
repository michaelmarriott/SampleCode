using System;
using System.Collections.Generic;
using Vox.Porta.Common.Account;
using Vox.Porta.Repository;

namespace Vox.Porta.Rest.Tests.Mocks {

  public class AccountMaintenanceRepositoryOracle : IAccountMaintenanceRepository {
    private Dictionary<string, double> entities;

    public AccountMaintenanceRepositoryOracle() {
      entities = new Dictionary<string, double>();
    }

    #region IAccountMaintenanceRepository Members

    public ICustomerRepository Customers { get; set; }

    public IAccountRepository Accounts { get; set; }

    public bool AddCreditExtension(AccountEntity account, double amount, Guid user) {
      return true;
    }

    public List<AccountEntity> ResetAllAccountBalances(IAccountRepository accountRepository, ICustomerRepository customerRepository) {
      return new List<AccountEntity>();
    }

    public void LoadBillingInformation(ref AccountEntity account) {
      if (entities.ContainsKey(account.Id)) {
        account.DefaultCreditLimit = entities[account.Id];
      } else {
        entities.Add(account.Id, account.DefaultCreditLimit);
      }
    }

    public List<AccountEntity> ResetAllAccountCreditExtensions() {
      return new List<AccountEntity>();
    }

    public AccountEntity GetByName(string name) {
      return null;
    }

    #endregion IAccountMaintenanceRepository Members

    #region IRepository<AccountEntity> Members

    public AccountEntity GetByID(int aID) {
      return null;
    }

    public bool Add(AccountEntity entity) {
      return true;
    }

    public bool Remove(AccountEntity entity) {
      return true;
    }

    public bool Update(AccountEntity entity) {
      if (entities.ContainsKey(entity.Id)) {
        entities[entity.Id] = entity.DefaultCreditLimit;
      } else {
        entities.Add(entity.Id, entity.DefaultCreditLimit);
      }
      return true;
    }

    #endregion IRepository<AccountEntity> Members

    #region IDisposable Members

    public void Dispose() {
    }

    #endregion IDisposable Members

    #region IAccountMaintenanceRepository Members

    public event AccountBalanceResetHandler AccountBalanceReset;

    #endregion IAccountMaintenanceRepository Members

    #region IAccountMaintenanceRepository Members

    public int GetIDByAssociatedNumber(string number) {
      throw new NotImplementedException();
    }

    #endregion IAccountMaintenanceRepository Members

    #region IAccountMaintenanceRepository Members

    public bool Add(AccountEntity entity, ICustomerRepository Customers) {
      return true;
    }

    #endregion IAccountMaintenanceRepository Members
  }
}