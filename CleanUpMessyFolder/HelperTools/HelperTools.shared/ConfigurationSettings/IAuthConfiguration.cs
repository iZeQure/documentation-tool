using HelperTools.shared.Models;

namespace HelperTools.shared.ConfigurationSettings
{
    /// <summary>
    /// Represents a generic authentication.
    /// </summary>
    public interface IAuthConfiguration
    {
        /// <summary>
        /// Gets the methods used to authenticate with given credentials.
        /// </summary>
        public LogInMethod LogInMethod { get; }

        /// <summary>
        /// Gets the user's credentials used for authentication.
        /// </summary>
        public Credentials Credentials { get; }
    }
}
