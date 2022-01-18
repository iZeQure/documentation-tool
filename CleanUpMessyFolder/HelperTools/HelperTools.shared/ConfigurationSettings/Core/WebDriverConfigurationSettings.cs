namespace HelperTools.shared.ConfigurationSettings.Core
{
    public class WebDriverConfigurationSettings : IWebConfiguration
    {
        public const string WebDriverConfig = "WebDriverConfig";

        /// <inheritdoc/>
        public string SiteUrl { get; set; }

        /// <inheritdoc/>
        public double ImplicitWaitTimeout { get; set; }

        /// <inheritdoc/>
        public double PageLoadTimeout { get; set; }

        /// <inheritdoc/>
        public double TimeoutInSeconds { get; set; }

        /// <inheritdoc/>
        public string Directory { get; set; }

        /// <inheritdoc/>
        public string[] Arguments { get; set; }
    }
}
