using System;

namespace HelperTools.shared
{
    /// <summary>
    /// An enum used for specifying a specific login method.
    /// </summary>
    public enum LogInMethod
    {
        UNI,
        NemID
    }

    public static class EnumToStringExtentions
    {
        public static string PrettyName(this LogInMethod logInMethod) => logInMethod switch
        {
            LogInMethod.UNI => Enum.GetName(typeof(LogInMethod), LogInMethod.UNI),
            LogInMethod.NemID => Enum.GetName(typeof(LogInMethod), LogInMethod.NemID),
            _ => throw new ArgumentOutOfRangeException(nameof(logInMethod), logInMethod, null)
        };
    }
}
