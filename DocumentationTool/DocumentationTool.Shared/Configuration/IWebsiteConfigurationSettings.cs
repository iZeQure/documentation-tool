namespace DocumentationTool.Shared.Configuration
{
    internal interface IWebsiteConfigurationSettings
    {
        int ImplicitWaitTimeout { get; }
        int PageLoadTimeout { get; }
        string Url { get; }
    }
}