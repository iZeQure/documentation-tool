namespace HelperTools.shared.Adapters
{
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message);

        void LogInformation<T0>(string message, T0 arg0);

        void LogInformation<T0, T1>(string message, T0 arg0, T1 arg1);

        void LogInformation<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
    }
}
