using System;
using System.Net;
using Imagine.Rest.PortaSwitch.Authentication;

namespace Imagine.Rest.Helper.Authentication {

  /// <summary>
  /// Authentication Helper class to authenticate against the PortaSwitch WebServices
  /// </summary>
  public class ServiceAuthentication {

    #region Private Variables

    /// <summary> User name who is trying to authenticate </summary>
    private string userName;

    /// <summary> Password used to authenticate the username </summary>
    private string password;

    #endregion Private Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceAuthentication" /> class.
    /// </summary>
    public ServiceAuthentication() {
      // This disables errors due to unregistered SSL Certificates
      ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
      // Load the configuration information into the object
      AuthenticationConfigSection config = (AuthenticationConfigSection)System.Configuration.ConfigurationManager.GetSection("portaAuthentication");
      userName = config.UserName;
      password = GetPassword();
      Environment = int.Parse(config.Environment);
    }

    #endregion Constructor0

    #region Properties

    /// <summary>
    /// Returns the UserName used to Authenticate against PortaSwitch
    /// </summary>
    public string UserName { get { return userName; } }

    /// <summary>
    /// Returns the Password used to Authenticate against PortaSwitch
    /// </summary>
    public string Password { get { return password; } }

    /// <summary> Gets the PortaOne environment to use </summary>
    public int Environment { get; private set; }

    #endregion Properties

    #region Private Methods

    /// <summary>
    /// Retrieves the password for the PortaSwitch system, based on the current month
    /// </summary>
    /// <returns>Returns the Password for the system based on the month</returns>
    private string GetPassword() {
      string password = String.Format("P0rt@{0}{1}", DateTime.Now.Month.ToString("00"), DateTime.Now.Year.ToString("0000"));
      return password;
    }

    #endregion Private Methods
  }
}