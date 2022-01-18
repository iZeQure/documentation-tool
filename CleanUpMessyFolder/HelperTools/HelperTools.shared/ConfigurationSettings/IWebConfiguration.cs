using System;

namespace HelperTools.shared.ConfigurationSettings
{
    public interface IWebConfiguration
    {
        /// <summary>
        /// Gets the url of the website.
        /// </summary>
        public string SiteUrl { get; }

        /// <summary>
        /// Gets the implicit wait timeout, which is the amount of time the driver
        /// should wait when searching for web elements.
        /// </summary>
        public double ImplicitWaitTimeout { get; }

        /// <summary>
        /// Gets the page load timeout, which is the amount of time the driver
        /// should wait for a website to load.
        /// </summary>
        public double PageLoadTimeout { get; }

        /// <summary>
        /// Gets the timeout in seconds.
        /// </summary>
        [Obsolete("This property is not up to date.")]
        public double TimeoutInSeconds { get; }

        /// <summary>
        /// Gets the root directory of the location of the web driver.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Gets the arguments provided to the web driver on load. This can be null.
        /// </summary>
        public string[] Arguments { get; }
    }
}
