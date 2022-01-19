namespace DocumentationTool.Shared.Configuration
{
    internal class WebsiteConfigurationSettings : IWebsiteConfigurationSettings
    {
        public string Url { get; set; }
        public int PageLoadTimeout { get; set; }
        public int ImplicitWaitTimeout { get; set; }
    }
}
