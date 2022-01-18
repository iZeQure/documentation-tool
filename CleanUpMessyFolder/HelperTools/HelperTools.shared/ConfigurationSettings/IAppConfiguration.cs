using HelperTools.shared.Models;

namespace HelperTools.shared.ConfigurationSettings
{
    /// <summary>
    /// Handles the configuration within the application. Data within this interface is populated from a configuration.
    /// </summary>
    public interface IAppConfiguration
    {
        /// <summary>
        /// Gets the information about the application document.
        /// </summary>
        /// <returns>The <see cref="IDocument"/>.</returns>
        public DocumentInformation ApplicationDocument { get; }

        /// <summary>
        /// Gets the information about the CV document.
        /// </summary>
        /// <returns>The <see cref="IDocument"/>.</returns>
        public DocumentInformation CVDocument { get; }
    }
}
