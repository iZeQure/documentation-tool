using HelperTools.shared.Models;
using System.Text.Json.Serialization;

namespace HelperTools.shared.ConfigurationSettings.Core
{
    /// <summary>
    /// Application configuration settings record type. Implementation of <see cref="IAppConfiguration"/>.
    /// </summary>
    public class ApplicationConfigurationSettings : IAppConfiguration
    {
        public const string AppConfig = "AppConfig";

        /// <inheritdoc cref="IAppConfiguration.ApplicationDocument"/>
        [JsonPropertyName("AppConfig:Documents:Application")]
        public DocumentInformation ApplicationDocument { get; private set; }

        /// <inheritdoc cref="IAppConfiguration.CVDocument"/>
        [JsonPropertyName("AppConfig:Documents:CV")]
        public DocumentInformation CVDocument { get; private set; }
    }
}
