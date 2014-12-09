using System.Collections.Generic;
using Vox.Porta.Common.DIDNumber;
using Vox.Porta.Repository;

namespace Vox.Porta.Rest.Tests.Mocks {

  public class DIDRepositoryOracle : IDIDRepository {

    #region IDIDRepository Members

    public Dictionary<int, DIDEntity> Load(int vendorID) {
      return new Dictionary<int, DIDEntity>();
    }

    public List<DIDEntity> Reserve(int vendorID, int amount, bool blockRequest) {
      List<DIDEntity> list = new List<DIDEntity>();
      list.Add(new DIDEntity(0, "A Mock DID"));
      return list;
    }

    #endregion IDIDRepository Members

    #region IRepository<DIDEntity> Members

    public DIDEntity GetByID(int aID) {
      return null;
    }

    public bool Add(DIDEntity entity) {
      return true;
    }

    public bool Remove(DIDEntity entity) {
      return true;
    }

    public bool Update(DIDEntity entity) {
      return true;
    }

    #endregion IRepository<DIDEntity> Members

    #region IDisposable Members

    public void Dispose() {
    }

    #endregion IDisposable Members
  }
}