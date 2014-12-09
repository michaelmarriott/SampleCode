using System.Configuration;

namespace Imagine.Rest.Helper.Authentication {

  /// <summary>
  /// Configuration section used for the PortaOne Authentication
  /// </summary>
  public class AuthenticationConfigSection : ConfigurationSection {

    /// <summary>
    /// Gets or sets the username used to login to the PortaOne System
    /// </summary>
    [ConfigurationProperty("UserName", IsRequired = true)]
    public string UserName {
      get {
        return this["UserName"].ToString();
      }
      set {
        this["UserName"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the environment value for the PortaOne System (This is critical for Database calls)
    /// </summary>
    [ConfigurationProperty("Environment", IsRequired = true)]
    public string Environment {
      get {
        return this["Environment"].ToString();
      }
      set {
        this["Environment"] = value;
      }
    }
  }
}