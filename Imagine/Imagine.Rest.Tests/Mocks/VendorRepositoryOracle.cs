
using Vox.Porta.Common.DIDNumber;
using Vox.Porta.Repository;

namespace Vox.Porta.Tests.Mocks {

  public class VendorRepositoryOracle : IVendorRepository {

    #region IVendorRepository Members

    public VendorEntity GetByName(string aName) {
      return new VendorEntity(1119, aName);
    }

    #endregion IVendorRepository Members

    #region IRepository<VendorEntity> Members

    public VendorEntity GetByID(int aID) {
      return null;
    }

    public bool Add(VendorEntity entity) {
      return true;
    }

    public bool Remove(VendorEntity entity) {
      return true;
    }

    public bool Update(VendorEntity entity) {
      return true;
    }

    #endregion IRepository<VendorEntity> Members

    #region IDisposable Members

    public void Dispose() {
    }

    #endregion IDisposable Members
  }
}