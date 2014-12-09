using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace Imagine.Rest.ViewModel {

  /// <summary>
  /// Base View Model class which contains the default properties that view models are expected to implement
  /// </summary>
  public abstract class BaseViewModel {

    /// <summary>
    /// Integer identifier used to represent the entity in the repository.
    /// </summary>
    /// <remarks>A integer identifier of 0 is assumed to be unsaved.</remarks>
    /// <exception cref="System.ArgumentException">Exception which is thrown when the id is less than 0</exception>
    [Required]
    public int InternalId { get; set; }

    /// <summary> Gets or sets the Unique name which is used to represent the entity </summary>
    /// <exception cref="System.ArgumentException">Thrown if the name is blank or null</exception>
    [Required]
    public string Id { get; set; }

    public override string ToString() {
      return JsonConvert.SerializeObject(this);
    }
  }
}