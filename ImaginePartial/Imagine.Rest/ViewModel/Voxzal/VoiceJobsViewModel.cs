using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Imagine.Rest.Data;
using Imagine.Rest.Model.Voxzal;

namespace Imagine.Rest.ViewModel.Voxzal {
  public class VoiceJobsViewModel {


    public Guid Id { get; set; }
    public string Name { get; set; }
    public dynamic Parameters { get; set; }
    public string Status { get; set; }
    public DateTime? LastUpdated { get; set; }

    public static explicit operator voice_job(VoiceJobsViewModel viewModel) {
      return new voice_job() {
        id = viewModel.Id,
        name = viewModel.Name,
        parameters = JsonConvert.SerializeObject(viewModel.Parameters),
        status = viewModel.Status,
        last_updated = viewModel.LastUpdated
      };
    }

    public static explicit operator VoiceJobsViewModel(voice_job model) {
      return new VoiceJobsViewModel() {
        Id = model.id,
        Name = model.name,
        Parameters = JsonConvert.DeserializeObject(model.parameters),
        Status = model.status,
        LastUpdated = model.last_updated
      };
    }

    static public VoiceJobsViewModel TestModel() {
      var followMeNumber = new VoiceJobsViewModel() {
        Id = Guid.NewGuid(),
        Name = "ReRating",
        Parameters = "{}",
        Status = "NEW",
        LastUpdated = DateTime.Now
      };
      return followMeNumber;
    }

    public override string ToString(){
      return JsonConvert.SerializeObject(this);
    }
  }
}