using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imagine.Rest.Data {
  public partial class voice_job : Imagine.Rest.Model.WebServiceModel<voice_job> {

    public voice_job() { }

    public List<voice_job> FindAll(string name, int limit, int offset) {    
      using (var voxzalModel = new VoxzalModel()){
        var result = (from v in voxzalModel.voice_job where v.name == name select v).OrderByDescending(x => x.last_updated).Take(limit).Skip(offset).ToList();
        return result;
      }
    }

    public voice_job Find(Guid id) {
      using (var voxzalModel = new VoxzalModel()) {
        var result = (from v in voxzalModel.voice_job where v.id == id select v).First();
        return result;
      }
    }

    public voice_job Find(int id) {
      throw new NotImplementedException();
    }

    public voice_job Find(string id) {
      throw new NotImplementedException();
    }

    public bool Create() {
      using (var context = new VoxzalModel()) {
        this.id = Guid.NewGuid();
        this.status = "NEW";
        context.Entry(this).State = System.Data.EntityState.Added;
        context.SaveChanges();
      }
      return true;
    }

    public bool Save() {
      using (var voxzalModel = new VoxzalModel()) {
        voxzalModel.Entry(this).State = System.Data.EntityState.Modified;
        voxzalModel.SaveChanges();
      }
      return true;
    }

    public bool Delete() {
      using (var voxzalModel = new VoxzalModel()) {
        voxzalModel.Entry(this).State = System.Data.EntityState.Deleted;
        voxzalModel.SaveChanges();
      }
      return true;
    }

  }
}