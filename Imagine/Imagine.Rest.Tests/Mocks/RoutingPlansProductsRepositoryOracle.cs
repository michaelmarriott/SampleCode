using System;
using Vox.Porta.Common.RoutingPlans;
using Vox.Porta.Repository;

namespace Vox.Porta.Rest.Tests.Mocks {

  internal class RoutingPlansProductsRepositoryOracle : IRoutingPlansProductsRepository {

    public RoutingPlan GetRoutingPlanByProductName(string productName) {
      return new RoutingPlan();
    }

    public RoutingPlan GetByID(int aID) {
      throw new NotImplementedException();
    }

    public bool Add(RoutingPlan entity) {
      throw new NotImplementedException();
    }

    public bool Remove(RoutingPlan entity) {
      throw new NotImplementedException();
    }

    public bool Update(RoutingPlan entity) {
      throw new NotImplementedException();
    }
  }
}