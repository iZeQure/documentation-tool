using DocumentationTool.Shared.Common;

namespace DocumentationTool.Shared.Configuration
{
    internal class AuthenticationSettings : IAuthenticationSettings
    {
        public AuthenticationMethod Method { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
