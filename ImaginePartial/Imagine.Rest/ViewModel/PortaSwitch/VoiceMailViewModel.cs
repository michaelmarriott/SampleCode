using Newtonsoft.Json;
using Imagine.Rest.PortaSwitch.VoiceMail;

namespace Imagine.Rest.ViewModel {

  /// <summary> VoiceMail View Model for the VoiceMail Model </summary>
   [JsonObject]
  public class VoiceMailViewModel {

    /// <summary>
    /// Gets or sets the Pin for voicemail account
    /// </summary>
    public string Pin { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the VoiceMail information is enabled for the account.
    /// </summary>
    public bool Enabled { get; set; }

    static public explicit operator VMSettings(VoiceMailViewModel voiceMailViewModel) {
      var vmSettings = new VMSettings();
      vmSettings.password = voiceMailViewModel.Pin;
      return vmSettings;
    }

    static public explicit operator VoiceMailViewModel(VMSettings vmSettings) {
      var voiceMailViewModel = new VoiceMailViewModel();
      voiceMailViewModel.Pin = vmSettings.password;
      return voiceMailViewModel;
    }

    /// <summary> Test Model Generator for Documentation </summary>
    /// <returns>A Test Object of VoiceMailViewModel</returns>
    static public VoiceMailViewModel TestModel() {
      var voiceMailViewModel = new VoiceMailViewModel() {
        Pin= "12345",
        Enabled = true
      };
      return voiceMailViewModel;
    }

  }
}