using HelperTools.logic.Handlers;
using HelperTools.shared.Adapters;
using HelperTools.shared.ConfigurationSettings;
using HelperTools.shared.ConfigurationSettings.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Diagnostics;

namespace AutomatisationBotTests.HandlerTests
{
    [TestFixture]
    public class AuthenticationHandlerTests
    {
        private IConfiguration _configuration;
        private IWebDriver _webDriver;
        private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
        });

        [SetUp]
        public void RunSetupBeforeAnyTests()
        {            
            Trace.Listeners.Add(new ConsoleTraceListener());

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.tests.json", optional: false, reloadOnChange: true)
                .Build();

            IWebConfiguration webConfiguration = _configuration.GetSection("WebDriverConfig").Get<WebDriverConfigurationSettings>();

            EdgeOptions edgeOptions = new();
            edgeOptions.AddArguments(webConfiguration.Arguments ?? Array.Empty<string>());

            _webDriver = new EdgeDriver(webConfiguration.Directory, edgeOptions)
            {
                Url = webConfiguration.SiteUrl
            };

            ITimeouts timeout = _webDriver.Manage().Timeouts();
            timeout.ImplicitWait = TimeSpan.FromSeconds(webConfiguration.ImplicitWaitTimeout);
            timeout.PageLoad = TimeSpan.FromSeconds(webConfiguration.PageLoadTimeout);
        }

        [TearDown]
        public void RunAfterTests()
        {
            if (_webDriver is null) return;

            _webDriver.Close();
            _webDriver.Dispose();

            Trace.Flush();
        }

        [Test]
        public void AuthenticateLogIn_ValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            IAuthenticationHandler handler = CreateHandler();

            bool expected = true;
            string message = "";

            // Act
            bool actual = false;

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                try
                {
                    actual = handler.AuthenticateLogIn();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }, message);

            Assert.IsEmpty(message, message);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("anonymous", "spaceship")]
        public void AuthenticateLogIn_InvalidCredentials_ShouldReturnFalse(string username, string password)
        {
            // Arrange
            IAuthenticationHandler handler = CreateHandler(username, password);

            bool expected = false;
            string message = "";

            // Act
            bool actual = true;

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                try
                {
                    actual = handler.AuthenticateLogIn();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }, message);

            Assert.IsEmpty(message, message);
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AuthenticateLogIn_ValidCredentials_ShouldFindValidSessionState()
        {
            // Arrange
            IAuthenticationHandler handler = CreateHandler();

            bool expected = true;

            // Act
            bool actualAuthenticationResult = false;

            // Assert
            Assert.DoesNotThrow(() =>
            {
                actualAuthenticationResult = handler.AuthenticateLogIn();
            });

            Assert.IsTrue(actualAuthenticationResult);
            Assert.IsTrue(handler.AuthenticationValid);
            Assert.AreEqual(expected, actualAuthenticationResult);
            Assert.AreEqual(expected, handler.AuthenticationValid);
        }

        [TestCase("anonymous", "spaceship")]
        public void AuthenticateLogIn_InvalidCredentials_ShouldNotHaveValidSessionState(string username, string password)
        {
            // Arrange
            IAuthenticationHandler handler = CreateHandler(username, password);

            // Act
            bool actualResult = handler.AuthenticateLogIn();

            // Assert
            Assert.IsFalse(actualResult);
            Assert.IsFalse(handler.AuthenticationValid);
        }

        private IAuthenticationHandler CreateHandler(string username = null, string password = null)
        {
            ILogger<AuthenticationHandler> logger = new Logger<AuthenticationHandler>(_loggerFactory);
            ILoggerAdapter<AuthenticationHandler> loggerAdapter = new LoggerAdapter<AuthenticationHandler>(logger);
            IAuthConfiguration authConfig = _configuration.GetSection("AuthConfig").Get<WebsiteAuthenticationConfigurationSettings>();

            authConfig.Credentials.Username = username ?? authConfig.Credentials.Username;
            authConfig.Credentials.Password = password ?? authConfig.Credentials.Password;

            AuthenticationHandler handler = new(_webDriver, loggerAdapter, authConfig);

            return handler;
        }
    }
}
