using DocumentationTool.Shared.Common;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentationTool.Prototypes.Tests2
{
    public class Settings
    {
        public Configuration Configuration { get; set; }
        public Authentication Authentication { get; set; }
    }

    public class Configuration
    {
        public string Website { get; set; }
        public byte PageLoadTimeoutInSeconds { get; set; }
        public byte ImplicitWaitTimeoutInSeconds { get; set; }
    }

    public class Authentication
    {
        public AuthenticationMethod Method { get; set; } = default;
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [TestFixture]
    public class PrototypeTest
    {
        private IWebDriver _webDriver;
        private WebDriverWait _driverWait;
        private const string _JSON_TEST_FILE = "appsettings.test.json";

        private Settings _settings;

        [OneTimeSetUp]
        public void Setup()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(_JSON_TEST_FILE, optional: false, reloadOnChange: true)
                .Build();

            _settings = configuration.GetSection("Settings").Get<Settings>();

            _webDriver = new EdgeDriver()
            {
                Url = _settings.Configuration.Website
            };

            _driverWait = new(_webDriver, TimeSpan.FromSeconds(5));

            // Configure timeouts.
            ITimeouts timeouts = _webDriver.Manage().Timeouts();
            timeouts.ImplicitWait = TimeSpan.FromSeconds(5);
            timeouts.PageLoad = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (_webDriver is null) return;

            _webDriver?.Close();
            _webDriver?.Quit();
            _webDriver?.Dispose();
        }

        [Test]
        [Order(1)]
        public void NavigateLogIn_LogInPage_ShouldNavigateToTheLogInPage()
        {
            // Arrange
            Uri logInUrl = CreateNavigationUriByRelativePath("login");

            // Act
            INavigation nav = _driverWait.Until(e => e.Navigate());
            nav.GoToUrl(logInUrl);

            // Assert
            Assert.AreEqual(logInUrl, _webDriver.Url);
        }

        [Test]
        [Order(2)]
        public void ChooseAuthenticationMethod_SupportedMethod_ShouldRedirectToChosenAuthenticationMethod()
        {
            // Arrange
            IWebElement authMethodElement = _webDriver.FindElements(By.TagName("button"))
                .FirstOrDefault(e => e.Text.Contains(_settings.Authentication.Method.GetName(), StringComparison.OrdinalIgnoreCase));

            Assert.NotZero(_settings.Authentication.Method.GetValue());
            Assert.IsNotNull(authMethodElement);
            authMethodElement.Click();
        }

        [Test]
        [Order(3)]
        public void EnterUsername_ValidArguments_ShouldNotDisplayAnyErrors()
        {
            Assert.DoesNotThrow(() => ValidateCredentials());

            // Arrange
            const string inputCssSelector = "input[name='username']";
            string errorMessage = "";

            // Act
            IWebElement inputField = _webDriver.FindElement(By.CssSelector(inputCssSelector));
            IWebElement submitBtn = FindSubmitButtonByText("næste");

            // Assert
            Assert.IsNotNull(inputField, "Input field couldn't be found.");
            inputField.SendKeys(_settings.Authentication.Username);

            Assert.IsNotNull(submitBtn, "Submit button couldn't be found.");
            submitBtn.Submit();

            Assert.Throws<NoSuchElementException>(() =>
            {
                IWebElement e = _webDriver.FindElement(By.Id("form-error-message-form-error"));

                errorMessage = e?.Text;
            }, errorMessage);

            Assert.IsEmpty(errorMessage);
        }

        [Test]
        [Order(4)]
        public void EnterPassword_ValidArguments_ShouldNotDisplayAnyErrors()
        {
            Assert.DoesNotThrow(() => ValidateCredentials());

            // Arrange
            const string inputCssSelector = "input[name='password']";

            // Act
            IWebElement inputField = _webDriver.FindElement(By.CssSelector(inputCssSelector));
            IWebElement submitBtn = FindSubmitButtonByText("log ind");
            string errorMessage = "";

            // Assert
            Assert.IsNotNull(inputField, "Input field couldn't be found.");
            inputField.SendKeys(_settings.Authentication.Password);

            Assert.IsNotNull(submitBtn, "Submit button couldn't be found.");
            submitBtn.Submit();

            Assert.Throws<NoSuchElementException>(() =>
            {
                IWebElement e = _webDriver.FindElement(By.Id("form-error-message-form-error"));

                errorMessage = e?.Text;
            }, errorMessage);

            Assert.IsEmpty(errorMessage);
        }

        [Test]
        [Order(5)]
        public void IsLoggedIn_AttempedLogIn_ShouldDisplayLoggedInUser()
        {
            // Arrange
            const string loggedInSectionCssSelector = "div[class*='Header_loggedInUser']";

            // Act
            IWebElement loggedInUser = null;

            Assert.DoesNotThrow(() =>
            {
                loggedInUser = _driverWait.Until(e => e.FindElement(By.CssSelector(loggedInSectionCssSelector)));
            });

            Assert.IsNotNull(loggedInUser);
        }

        [TestCase("It operators")]
        [Order(6)]
        public void FindApprenticeshipByFreeText_ValidText_ShouldElementContainingTheApprenticeship(string text)
        {
            // Arrange
            const string url = "https://pms.xn--lrepladsen-d6a.dk/soeg-opslag";
            const string inputFieldCssSelector = "input[name='fritekst']";

            // Act
            _webDriver.Navigate().GoToUrl(url);

            IWebElement inputField = _webDriver.FindElement(By.CssSelector(inputFieldCssSelector));
            inputField.SendKeys(text);

            IWebElement searchBtn = _webDriver.FindElements(By.TagName("button"))
                .FirstOrDefault(e => e.Text.Contains("søg", StringComparison.OrdinalIgnoreCase));
            searchBtn.Click();

            try
            {
                _ = _webDriver.FindElement(By.CssSelector("div[class*='Alert']"));

                Assert.Fail("No Apprenticeship found.");
            }
            catch (NoSuchElementException)
            {
                // Ignore
            }
        }

        [TestCase("It operators")]
        [Order(7)]
        public void BookmarkApprenticeship_MatchesCriteria_ShouldBookmarkTheResult(string text)
        {
            const string unbookmarkedData = "M17 3H7c-1.1 0-2 .9-2 2v16l7-3 7 3V5c0-1.1-.9-2-2-2zm0 15l-5-2.18L7 18V5h10v13z";

            IEnumerable<IWebElement> results = _webDriver.FindElements(By.CssSelector("div[class*='SoegOpslag_searchResultEntry'"))
                .Where(e => e.Text.Contains(text, StringComparison.OrdinalIgnoreCase));

            foreach (IWebElement item in results)
            {
                IWebElement bookmark = item.FindElement(By.CssSelector("div[class*='AnsoegningslogBookmark_container']"));

                IReadOnlyCollection<IWebElement> paths = bookmark.FindElements(By.TagName("path"));

                if (paths.ElementAt(1).GetAttribute("d").Equals(unbookmarkedData))
                {
                    bookmark.Click();
                    continue;
                }

                Console.WriteLine("Apprenticeship is already bookmarked.");
            }
        }

        private Uri CreateNavigationUriByRelativePath(string relativePath) =>
            new(_webDriver.Url + relativePath);

        private IWebElement FindSubmitButtonByText(string text)
            => _driverWait.Until(e => e.FindElements(By.CssSelector("button[type='submit']"))
            .FirstOrDefault(x => x.Text.Contains(text, StringComparison.OrdinalIgnoreCase)));

        private void ValidateCredentials()
        {
            if (string.IsNullOrEmpty(_settings.Authentication.Username)
                | string.IsNullOrEmpty(_settings.Authentication.Password))
            {
                throw new NullReferenceException("Invalid credentials during authentication.");
            }
        }

    }
}
