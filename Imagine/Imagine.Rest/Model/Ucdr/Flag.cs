using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Imagine.Rest.Data;

namespace Imagine.Rest.Data {
  public partial class FLAG : Imagine.Rest.Model.WebServiceModel<FLAG> {

    public List<FLAG> FindByName(string name, int limit, int offset, string orderby) {
      var flags = new List<FLAG>();
      using (var context = new DrEntity()) {
        var result = (from f in context.FLAGs
                      where f.NAME == name && f.DATEFROM != null
                      select f).OrderByDescending(x => x.FLAGID).Take(limit).Skip(offset);
        flags = result.ToList();
      }
      return flags;
    }

    public FLAG Find(int id) {
      FLAG flag = null;
      using (var context = new DrEntity()) {
        flag = context.FLAGs.First(x => x.FLAGID == id);
      }
      return flag;
    }

    public FLAG Find(string id) {
      throw new NotImplementedException();
    }

    public bool Create() {
      string sqlSequence = @"select FLAG_SEQ.NEXTVAL FROM dual";
      using (var context = new DrEntity()) {
        int id = context.Database.SqlQuery<int>(sqlSequence).FirstOrDefault<int>();
        this.FLAGID = id;
        context.Entry(this).State = System.Data.EntityState.Added;
        context.SaveChanges();
      }
      return true;
    }

    public bool Save() {
      using (var context = new DrEntity()) {
        context.Entry(this).State = EntityState.Modified;
        context.SaveChanges();
      }
      return true;
    }

    public bool Delete() {
      using (var context = new DrEntity()) {
        context.Entry(this).State = EntityState.Deleted;
        context.SaveChanges();
      }
      return true;
    }
  }
}