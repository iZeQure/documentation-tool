namespace DocumentationTool.Shared.Handlers
{
    /// <summary>
    /// Represents a generic authentication handler. 
    /// Provides access for authentication properties and methods.
    /// </summary>
    public interface IAuthenticationHandler
    {
        public bool AuthenticationSuccess { get; }

        bool AuthenticateLogIn();
    }
}
