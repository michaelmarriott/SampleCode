using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Imagine.Rest.Data {
  public partial class CLOUD : Imagine.Rest.Model.WebServiceModel<CLOUD> {

    public CLOUD() {

    }

    public List<CLOUD> FindByName(string name, int limit, int offset) {
      List<CLOUD> clouds = null;
      using (var context = new DrEntity()) {
        var result = (from f in context.CLOUDs
                      where f.NAME == name
                      select f).OrderBy(x => x.CLOUDID).Take(limit).Skip(offset);
        clouds = result.ToList();
      }
      return clouds;
    }

    public CLOUD Find(int id) {
      CLOUD cloud = null;
      using (var context = new DrEntity()) {
        var result = (from f in context.CLOUDs
                      where f.CLOUDID == id
                      select f).OrderBy(x => x.CLOUDID);
        cloud = result.First();
      }
      return cloud;
    }

    public CLOUD Find(string id) {
      throw new NotImplementedException();
    }

    public List<CLOUD> FindAll(int offset = 0, int limit = 10) {
      List<CLOUD> clouds = null;
      using (var context = new DrEntity()) {
        var result = (from f in context.CLOUDs
                      select f).OrderByDescending(x => x.CLOUDID).Take(limit).Skip(offset);
        clouds = result.ToList();
      }
      return clouds;
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