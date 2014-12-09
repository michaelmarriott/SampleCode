using System;
using Vox.Porta.Common.Response;
using Vox.Porta.Repository;

namespace Vox.Porta.Rest.Tests.Mocks {

  public class ResponseRepositoryOracle : IResponseRepository {

    #region IResponseRepository Members

    public JsonResponse GetByGuid(Guid aGuid, string command) {
      return new JsonResponse(Guid.NewGuid());
    }

    public void Initialise(dynamic aContext) {
    }

    #endregion IResponseRepository Members

    #region IRepository<Response> Members

    public JsonResponse GetByID(int aID) {
      return new JsonResponse(Guid.NewGuid());
    }

    public bool Add(JsonResponse entity) {
      return true;
    }

    public bool Remove(JsonResponse entity) {
      return true;
    }

    public bool Update(JsonResponse entity) {
      return true;
    }

    #endregion IRepository<Response> Members
  }
}