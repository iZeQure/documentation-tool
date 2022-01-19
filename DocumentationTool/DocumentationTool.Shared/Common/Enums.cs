using System;

namespace DocumentationTool.Shared.Common
{
    public enum AuthenticationMethod
    {
        None = 0,
        UNI,
        NEMID,
        MITID
    }

    [Obsolete("This method is outdated.")]
    public enum LogInMethod
    {
        UNI,
        NemID
    }

    public static class EnumExtentions
    {
        const string NotSupportedExceptionMessage = "Method is not supported, by given arguments.";

        public static string GetName(this AuthenticationMethod authenticationMethod) =>
            authenticationMethod switch
            {
                AuthenticationMethod.None => Enum.GetName(authenticationMethod),
                AuthenticationMethod.UNI => Enum.GetName(authenticationMethod),
                AuthenticationMethod.NEMID => Enum.GetName(authenticationMethod),
                AuthenticationMethod.MITID => Enum.GetName(authenticationMethod),
                _ => throw new NotSupportedException(NotSupportedExceptionMessage)
            };

        [Obsolete("This method is outdated.")]
        public static string GetName(this LogInMethod logInMethod) =>
            logInMethod switch
            {
                LogInMethod.UNI => Enum.GetName(logInMethod),
                LogInMethod.NemID => Enum.GetName(logInMethod),
                _ => throw new NotSupportedException(NotSupportedExceptionMessage),
            };

        public static int GetValue(this AuthenticationMethod authenticationMethod) =>
            authenticationMethod switch
            {
                AuthenticationMethod.None => (int)authenticationMethod,
                AuthenticationMethod.UNI => (int)authenticationMethod,
                AuthenticationMethod.NEMID => (int)authenticationMethod,
                AuthenticationMethod.MITID => (int)authenticationMethod,
                _ => throw new NotSupportedException(NotSupportedExceptionMessage),
            };

        [Obsolete("This method is outdated.")]
        public static int GetValue(this LogInMethod loginMethod) =>
            loginMethod switch
            {
                LogInMethod.UNI=> (int)loginMethod,
                LogInMethod.NemID=> (int)loginMethod,
                _ => throw new NotSupportedException(NotSupportedExceptionMessage),
            };
    }
}
