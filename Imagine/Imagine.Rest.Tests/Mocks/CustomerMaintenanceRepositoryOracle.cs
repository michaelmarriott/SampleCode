namespace Vox.Porta.Rest.Tests.Mocks {

  /// <summary>
  /// TODO: Update summary.
  /// </summary>
  public class CustomerMaintenanceRepositoryOracle : ICustomerMaintenanceRepository {

    #region ICustomerMaintenanceRepository Members

    public bool AddCreditExtension(CustomerEntity customer, System.Guid user) {
      return true;
    }

    public int GetBillingDay(int id, string name) {
      return 1;
    }

    public double GetCreditExtension(CustomerEntity customer) {
      return 0;
    }

    #endregion ICustomerMaintenanceRepository Members

    #region IRepository<CustomerEntity> Members

    public CustomerEntity GetByID(int aID) {
      throw new System.NotImplementedException();
    }

    public bool Add(CustomerEntity entity) {
      throw new System.NotImplementedException();
    }

    public bool Remove(CustomerEntity entity) {
      throw new System.NotImplementedException();
    }

    public bool Update(CustomerEntity entity) {
      throw new System.NotImplementedException();
    }

    #endregion IRepository<CustomerEntity> Members
  }
}