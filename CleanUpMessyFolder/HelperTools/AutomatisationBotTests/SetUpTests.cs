//using HelperTools.shared.ConfigurationSettings.Core;
//using Microsoft.Extensions.Configuration;
//using NUnit.Framework;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Edge;
//using System;

//namespace AutomatisationBotTests
//{
//    [SetUpFixture]
//    public class SetUpTests
//    {
//        private IConfiguration _configuration;
//        private IWebDriver _webDriver;

//        public IConfiguration Configuration => _configuration;

//        public IWebDriver WebDriver => _webDriver;

//        [OneTimeSetUp]
//        public void RunSetupBeforeAnyTests()
//        {
//            // Arrange
//            ITimeouts driverTimeouts = null;
//            EdgeOptions edgeOptions = null;

//            _configuration = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.tests.json", optional: false, reloadOnChange: true)
//                .Build();

//            string[] driverArgs = Configuration.GetSection("WebDriverConfig:Arguments")
//                .Get<string[]>() ?? Array.Empty<string>();

//            // Act & Assert
//            Assert.IsNotNull(_configuration);

//            Assert.DoesNotThrow(() =>
//            {
//                edgeOptions = new();
//                edgeOptions.AddArguments(driverArgs);

//                _webDriver = new EdgeDriver(Configuration["WebDriverConfig:Directory"], edgeOptions)
//                {
//                    //Url = new Uri(Configuration["WebDriverConfig:SiteUri"]).AbsoluteUri
//                };

//                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
//                _webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

//                //bool parsedTimeoutFromConfig = double.TryParse(Configuration["WebDriverConfig:TimeoutInSeconds"], out double timeout);
//                //driverTimeouts = _webDriver.Manage().Timeouts();
//                //driverTimeouts.ImplicitWait = TimeSpan.FromSeconds(parsedTimeoutFromConfig ? timeout : 10);
//                //driverTimeouts.PageLoad = TimeSpan.FromSeconds(parsedTimeoutFromConfig ? timeout : 10);
//            }, "Web Driver threw an exception during initialization.");

//            Assert.IsNotNull(_webDriver, "Web Driver was null.");
//        }

//        [OneTimeTearDown]
//        public void RunAfterTests()
//        {
//            if (_webDriver is null) return;

//            _webDriver.Close();
//            _webDriver.Dispose();
//        }
//    }
//}
