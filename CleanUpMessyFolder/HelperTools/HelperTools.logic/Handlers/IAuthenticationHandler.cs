namespace HelperTools.logic.Handlers
{
    /// <summary>
    /// Represents a generic authentication handler, 
    /// exposing the methods and properties for handling an authentication.
    /// </summary>
    public interface IAuthenticationHandler
    {
        /// <summary>
        /// Get the authentication state, is true if an authentication is valid. Default false.
        /// </summary>
        bool AuthenticationValid { get; }

        /// <summary>
        /// Authenticates the login process.
        /// </summary>
        /// <returns>True if the authentication is successful, otherwise false.</returns>
        bool AuthenticateLogIn();
    }
}