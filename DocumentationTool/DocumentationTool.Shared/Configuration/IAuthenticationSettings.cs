using DocumentationTool.Shared.Common;

namespace DocumentationTool.Shared.Configuration
{
    internal interface IAuthenticationSettings
    {
        AuthenticationMethod Method { get;}
        string Password { get; }
        string Username { get; }
    }
}