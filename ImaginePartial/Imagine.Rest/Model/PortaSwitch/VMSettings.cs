using System;
using Imagine.Rest.Helper.Authentication;
using Imagine.Rest.Model;

namespace Imagine.Rest.PortaSwitch.VoiceMail {

  /// <summary>
  /// Voice Mail Web Service Model Partial
  /// </summary>
  public partial class VMSettings : WebServiceModel<VMSettings> {
    #region Properties
    public string AccountId { get; set; }

    public string AccountPassword { get; set; }
    #endregion

    /// <summary>
    /// Save the VoiceMail settings for the given VoiceMail Model
    /// </summary>
    /// <returns>True if the settings are saved, otherwise false</returns>
    public bool Save() {
      var config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      int enviroment = int.Parse(config.Environment);
      try {
        using (var service = new VoicemailService()) {
          SetAuthentication(AccountId, AccountPassword, service);
          var voiceMailSettings = service.get_vm_settings(new GetVMSettingsRequest());
          voiceMailSettings.vm_settings.password = password;
          var voiceMailResult = service.set_vm_settings(new SetVMSettingsRequest() { vm_settings = voiceMailSettings.vm_settings });
          if (voiceMailResult.vm_settings_saved != 0) {
            return true;
          }
        }
      } catch (Exception ex) {
        if (enviroment == 4) {
          return true;
        }
        throw ex;
      }
      return false;
    }

    public bool Create() { throw new System.NotImplementedException(); }

    public VMSettings Find(int id) { throw new System.NotImplementedException(); }

    public VMSettings Find(string id) {
      var config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      int enviroment = int.Parse(config.Environment);
      try {
        using (var service = new VoicemailService()) {
          SetAuthentication(id, AccountPassword, service);
          var voiceMailSettingResponse = service.get_vm_settings(new GetVMSettingsRequest() { });
          var voiceMailSetting = voiceMailSettingResponse.vm_settings;
          return voiceMailSetting;
        }
      }catch(Exception ex){
        if (enviroment == 4) {
          return StagingFaked(id, AccountPassword);
        }
        throw ex;
      }
    }

    public bool Delete() { throw new System.NotImplementedException(); }

    private void SetAuthentication(string login, string password, VoicemailService service) {
      service.AuthInfoStructureValue = new AuthInfoStructure() {
        login = login,
        password = password,
        domain = "ms1.vmail.co.za"
      };
    }

    //No staging enviroment, thus will always fail, thus return fake
    private VMSettings StagingFaked(string id, string password) {
      return new VMSettings() { AccountId = id, AccountPassword = password, password = "12345" };
    }

  }
}