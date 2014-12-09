using Imagine.Rest.PortaSwitch.VoiceMail;
using Imagine.Rest.PortaSwitch.VoiceMail.Fakes;

namespace Imagine.Rest.Tests.Mocks {
  public class VoicemailService {
    public static void get_vm_settings() {
      ShimVoicemailService.AllInstances.get_vm_settingsGetVMSettingsRequest = (c, request) => {
        return new GetVMSettingsResponse() { vm_settings = new VMSettings() };
      };
    }

    public static void set_vm_settings() {
      ShimVoicemailService.AllInstances.set_vm_settingsSetVMSettingsRequest = (c, request) => {        
        return new SetVMSettingsResponse() { vm_settings_saved = 1 };        
      };
    }
  }
}
