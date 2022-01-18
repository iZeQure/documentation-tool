using HelperTools.shared.Models;

namespace HelperTools.shared.ConfigurationSettings.Core
{
    public class WebsiteAuthenticationConfigurationSettings : IAuthConfiguration
    {
        public const string AuthConfig = "AuthConfig";

        public LogInMethod LogInMethod { get; set; }

        public Credentials Credentials { get; set; }
    }
}
