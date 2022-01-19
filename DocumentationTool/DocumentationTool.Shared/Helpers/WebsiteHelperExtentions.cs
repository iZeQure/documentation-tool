using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace DocumentationTool.Shared.Helpers
{
    public static class WebsiteHelperExtentions
    {
        private const int _helperTimeout = 5;

        /// <summary>
        /// Goes to the url, by a given relative path.
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="relativePath">Contains the path to associate at the end of the url.</param>
        /// <returns>True when the location has changed, otherwise false.</returns>
        public static bool GoToUrlByRelativePath(this IWebDriver webDriver, string relativePath)
        {
            Uri url = new(webDriver.Url + relativePath);

            webDriver.Navigate().GoToUrl(url);

            WebDriverWait wait = new(webDriver, TimeSpan.FromSeconds(_helperTimeout));

            return wait.Until<bool>(driver => driver.Url.Contains(relativePath));
        }
    }
}
