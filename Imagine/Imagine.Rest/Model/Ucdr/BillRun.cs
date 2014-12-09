using System;
using System.Linq;
using System.Collections.Generic;

namespace Imagine.Rest.Data {
  public partial class BILLRUN : Imagine.Rest.Model.WebServiceModel<BILLRUN> {

    public BILLRUN Find(int id) {
      BILLRUN billRun = null;
      using (var context = new DrEntity()) {
        var result = (from f in context.BILLRUNs
                      where f.BILLRUNID == id
                      select f);
        billRun = result.First();
      }
      return billRun;
    }

    public BILLRUN Find(string id) {
      throw new NotImplementedException();
    }

    public List<BILLRUN> FindAll(int lobId, string code, int? isMidMonth, int limit = 10, int offset = 0) {
      List<BILLRUN> billRuns = null;
      using (var context = new DrEntity()) {
        IQueryable<BILLRUN> result = null;
        result = (from f in context.BILLRUNs select f);
        if (lobId != 0) {
          var lobId_q = (decimal)lobId;
          result = (from f in result where f.LOBID == lobId_q select f);
        }
        if (code != null) {
          result = (from f in result where f.CODE == code select f);
        }
        if (isMidMonth != null) {
          result = (from f in result where f.ISMIDMONTH == isMidMonth select f);
        }
        billRuns = result.OrderByDescending(x => x.LOBID).OrderByDescending(x => x.CODE).Take(limit).Skip(offset).ToList();
      }
      return billRuns;
    }

    public bool Create() {
      using (var context = new DrEntity()) {
        context.Entry(this).State = System.Data.EntityState.Added;
        context.SaveChanges();
      }
      return true;
    }

    public bool Save() {
      using (var context = new DrEntity()) {
        context.Entry(this).State = System.Data.EntityState.Modified;
        context.SaveChanges();
      }
      return true;
    }

    public bool Delete() {
      using (var context = new DrEntity()) {
        context.Entry(this).State = System.Data.EntityState.Deleted;
        context.SaveChanges();
      }
      return true;
    }
  }
}