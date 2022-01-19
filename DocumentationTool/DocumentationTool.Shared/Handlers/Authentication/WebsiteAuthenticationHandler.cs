using DocumentationTool.Shared.Common;
using DocumentationTool.Shared.Configuration;
using DocumentationTool.Shared.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentationTool.Shared.Handlers.Authentication
{
    /// <summary>
    /// Handles the authentication on a specific website.
    /// </summary>
    internal class WebsiteAuthenticationHandler : IAuthenticationHandler
    {
        private readonly IWebDriver _webDriver;
        private readonly IAuthenticationSettings _authenticationSettings;

        private bool _authenticationSuccess = false;

        public WebsiteAuthenticationHandler(IAuthenticationSettings authenticationSettings, IWebDriver webDriver)
        {
            _authenticationSettings = authenticationSettings;
            _webDriver = webDriver;
        }

        public bool AuthenticationSuccess => _authenticationSuccess;

        public bool AuthenticateLogIn()
        {
            try
            {
                bool enteredLogInPath = _webDriver.GoToUrlByRelativePath("login");

                if (enteredLogInPath is false)
                {
                    return _authenticationSuccess;
                }

                ChooseLogInMethod();

                bool isAuthenticated = SubmitLoginForm();

                if (isAuthenticated is true)
                {
                    VerifyValidSessionState();
                }

                return _authenticationSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred during {0} : {1}", nameof(AuthenticateLogIn), ex.Message);
                return _authenticationSuccess;
            }
        }

        /// <summary>
        /// Validates the session after the authentication has occurred successfully.
        /// </summary>
        private void VerifyValidSessionState()
        {
            const string sessionCssSelector = "div[class*='loggedInUser']";

            try
            {
                _ = _webDriver.FindElement(By.CssSelector(sessionCssSelector));

                _authenticationSuccess = true;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Session is invalid. Authentication unsuccessful.");
                _authenticationSuccess = false;
            }
        }



        /// <summary>
        /// Locates the login function and clicks it if found.
        /// </summary>
        /// <exception cref="NullReferenceException">Is thrown if the login element is not found.</exception>
        [Obsolete("No longer supported.")]
        private void LocateLogInElement()
        {
            IWebElement loginFunction = null;
            const string tagName = "button";
            const string functionText = "log ind";

            try
            {
                IReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(By.TagName(tagName));

                loginFunction = elements.FirstOrDefault(
                    element =>
                    element.Text
                    .Contains(functionText, StringComparison.OrdinalIgnoreCase));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Failed to locate the login function.");

                throw new NullReferenceException("Failed to locate the login function.");
            }

            loginFunction.Click();
        }

        /// <summary>
        /// Chooses the specific login method and submits it.
        /// </summary>
        /// <exception cref="NotFoundException">Is thrown if no login methods are found.</exception>
        /// <exception cref="System.NotSupportedException">Is thrown if the selected login method is not supported.</exception>
        private void ChooseLogInMethod()
        {
            const string btnTagName = "button";
            string loginMethod = _authenticationSettings.Method.GetName();

            try
            {
                IReadOnlyCollection<IWebElement> supportedLoginMethods = _webDriver.FindElements(By.TagName(btnTagName));

                if (supportedLoginMethods is null || !supportedLoginMethods.Any())
                {
                    throw new NotFoundException("No login methods are found.");
                }

                IWebElement loginMethodElement = supportedLoginMethods.FirstOrDefault(
                    element =>
                    element.Text
                    .Contains(loginMethod, StringComparison.OrdinalIgnoreCase));

                if (loginMethodElement is null)
                {
                    throw new NotSupportedException($"No login method is found, using {loginMethod}.");
                }

                loginMethodElement.Submit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Submits the login form by login method.
        /// </summary>
        /// <returns>True if the chosen login method was submitted without errors, otherwise false.</returns>
        private bool SubmitLoginForm()
        {
            const string usernameFormCssSelector = "input[name='username']";
            const string passwordFormCssSelector = "input[name='password']";

            try
            {
                IWebElement usernameInputField = _webDriver.FindElement(By.CssSelector(usernameFormCssSelector));
                usernameInputField?.SendKeys(_authenticationSettings.Username);

                IWebElement nextSubmitBtn = FindSubmitButtonByText("næste");
                nextSubmitBtn?.Submit();

                bool usernameIsValid = EnteredValidFormField();

                if (usernameIsValid is false)
                {
                    return false;
                }

                IWebElement passwordInputField = _webDriver.FindElement(By.CssSelector(passwordFormCssSelector));
                passwordInputField?.SendKeys(_authenticationSettings.Password);

                IWebElement loginSubmitBtn = FindSubmitButtonByText("log ind");
                loginSubmitBtn?.Submit();

                bool passwordIsValid = EnteredValidFormField();

                if (passwordIsValid is false)
                {
                    return false;
                }

                return true;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Username or password field was not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception occurred, with message: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates if the entered value is valid in the form.
        /// </summary>
        /// <returns>True if not error message occurred, otherwise false.</returns>
        private bool EnteredValidFormField()
        {
            try
            {
                //_webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                const string errorMessageCssId = "form-error-message-form-error";

                _ = _webDriver.FindElement(By.CssSelector(errorMessageCssId));

                return false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        /// <summary>
        /// Finds the submit button by text.
        /// </summary>
        /// <param name="text">The text in the submit button, used for finding the right one. This param is set to lowercase.</param>
        /// <returns>A <see cref="IWebElement"/> containing the submit button if the text in the button contains the <paramref name="text"/>, otherwise null.</returns>
        private IWebElement FindSubmitButtonByText(string text)
        {
            const string submitBtnCssSelector = "button[type='submit']";

            try
            {
                IReadOnlyCollection<IWebElement> submitButtons = _webDriver.FindElements(By.CssSelector(submitBtnCssSelector));
                IWebElement submitButton = submitButtons?.Any() == true
                    ? submitButtons.FirstOrDefault(e => e.Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                    : null;

                return submitButton;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception occurred: {0}", ex.Message);

                return null;
            }
        }
    }
}
