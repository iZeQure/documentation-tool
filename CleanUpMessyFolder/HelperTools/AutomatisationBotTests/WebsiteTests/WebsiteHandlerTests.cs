//using NUnit.Framework;
//using OpenQA.Selenium;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;

//namespace AutomatisationBotTests.WebsiteTests
//{
//    [TestFixture]
//    public class WebsiteHandlerTests : SetUpTests
//    {
//        [Test]
//        [Order(1)]
//        public void AutomaticAuthentication_ChecksSession_ShouldLoginIfNoUserIsLoggedIn()
//        {
//            // Arrange

//            // Assert
//            Assert.DoesNotThrow(() =>
//            {
//                try
//                {
//                    LogInOnWebsite();
//                }
//                catch (Exception ex)
//                {
//                    Assert.Fail(ex.Message);
//                }
//            });

//            //Assert.Pass(testPassedText);
//        }

//        [Test]
//        [Order(2)]
//        public void CheckAuthentication_HasBeenAuthenticated_ShouldDisplayLoggedInUser()
//        {
//            // Arrange
//            const string loggedInClassName = "Header_loggedInUser__FU8J_";

//            IWebElement actualLoginState = null;

//            // Act & Assert
//            Assert.DoesNotThrow(() =>
//            {
//                try
//                {
//                    LogInOnWebsite();
//                }
//                catch (Exception ex)
//                {
//                    Assert.Fail(ex.Message);
//                }

//                actualLoginState = WebDriver.FindElement(By.ClassName(loggedInClassName));
//            });

//            Assert.IsNotNull(actualLoginState);
//            Assert.IsTrue(actualLoginState.Displayed, "Session is valid.");
//        }

//        [TestCase("dagrofa")]
//        [Order(4)]
//        public void SearchApprenticeship_ValidArguments_ShouldSearchAfterASpecificEducation(string companyName)
//        {
//            const string apprenticeshipLinkText = "søg læreplads";
//            const string inputFreeTextFieldName = "fritekst";
//            const string submitSearchBtnText = "søg";

//            // Refresh page to make sure it's updated.
//            WebDriver.Navigate().Refresh();

//            IReadOnlyCollection<IWebElement> webLinks = WebDriver.FindElements(By.TagName("a"));
//            var apprenticeshipLink = webLinks.FirstOrDefault(e => e.Text.ToLower().Contains(apprenticeshipLinkText));

//            if (apprenticeshipLink is null)
//            {
//                Assert.Fail("Link is not found.");
//            }

//            apprenticeshipLink.Click();

//            IWebElement freeTextInput = WebDriver.FindElement(By.Name(inputFreeTextFieldName));
//            freeTextInput?.SendKeys(companyName);
//            IWebElement searchBtn = WebDriver.FindElements(By.TagName("button"))
//                .FirstOrDefault(e => e.Text.ToLower().Contains(submitSearchBtnText));
//            searchBtn?.Submit();
//        }

//        [TestCase("infrastruktur")]
//        [Order(5)]
//        public void AddBookmark_FoundCompany_ShouldAddTheCompanyAsBookmarkIfFoundAndNotAlreadyBookmarked(string educationSuffix)
//        {
//            // Arrange
//            const string bookmarkedAttributeValue = "M17 3H7c-1.1 0-2 .9-2 2v16l7-3 7 3V5c0-1.1-.9-2-2-2z";
//            int bookmarkCount = default;

//            try
//            {
//                bool? companyFound =
//                    WebDriver.FindElements(
//                    By.CssSelector("[class*='Alert_info']")).FirstOrDefault()
//                    ?.Text.ToLower().Contains("ingen resultater");

//                if (companyFound is not null or false)
//                {
//                    Assert.Fail("Company was not found.");
//                }
//            }
//            catch (NoSuchElementException)
//            {
//                // Ignore Exception.
//            }

//            // Find all companies with same education range.
//            IReadOnlyCollection<IWebElement> categorisedCompanies =
//                WebDriver.FindElements(By.CssSelector($"[class*='{"SoegOpslag_searchResultEntry"}']"));

//            if (categorisedCompanies is null)
//            {
//                Assert.Fail("Did not find any companies with specified name.");
//            }

//            IEnumerable<IWebElement> bookmarkContainers =
//                categorisedCompanies.Where(e => e.Text.ToLower().Contains(educationSuffix));

//            foreach (IWebElement container in bookmarkContainers)
//            {
//                IReadOnlyCollection<IWebElement> bookmarkSVGPaths = container.FindElements(By.TagName("path"));

//                string pathAttributeValue = bookmarkSVGPaths.ElementAt(1).GetAttribute("d");

//                if (pathAttributeValue.Equals(bookmarkedAttributeValue))
//                {
//                    Console.WriteLine($"This one is already bookmarked.");
//                    continue;
//                }

//                IWebElement bookmark = container.FindElement(By.CssSelector($"[class*='AnsoegningslogBookmark_container']"));

//                bookmark?.Click();

//                bookmarkCount++;

//                bookmark?.Click();
//            }

//            Assert.Greater(bookmarkCount, 0);
//        }

//        [Test]
//        [Order(6)]
//        public void UploadFile_ValidSearchLog_ShouldUploadFileToAddedBookmark()
//        {
//            LogInOnWebsite();

//            // Arrange
//            var cv = Configuration.GetSection("AppConfig:Documents:CV");
//            string file = Path.Combine(cv["Location"], cv["FileName"]);

//            const string cssSelector = "[class*='TextButton_textButton']";

//            // Locate the search log.
//            IWebElement searchLog = WebDriver.FindElements(By.CssSelector(cssSelector))
//                .Select(e => e)
//                .FirstOrDefault(e => !string.IsNullOrEmpty(e.Text) && e.Text.ToLower().Contains("vis søgelog"));

//            searchLog?.Click();

//            // Find the bookmarked company, in the search log.
//            IEnumerable<IWebElement> bookmarkedCompanies = WebDriver.FindElements(By.CssSelector(cssSelector))
//                .Select(e => e)
//                .Where(e => !string.IsNullOrEmpty(e.Text));

//            if (bookmarkedCompanies is not null)
//            {
//                //IWebElement firstBtn = bookmarkedCompanies?.FirstOrDefault();

//                foreach (IWebElement item in bookmarkedCompanies)
//                {
//                    if (item is null)
//                    {
//                        continue;
//                    }

//                    string itemText = item.Text.ToLower();
//                    if (itemText.Contains("tilføj"))
//                    {
//                        Console.WriteLine("Found add button");
//                        item.Click();
//                        UploadFileToInputField();

//                        Thread.Sleep(100);
//                    }
//                }
//            }
//        }

//        private void LogInOnWebsite()
//        {
//            // Arrange
//            NavigateTo(Configuration["WebDriverConfig:SiteUrl"]);

//            IWebElement userLogin = WebDriver
//                .FindElements(By.TagName("button"))
//                .FirstOrDefault(e => e.Text.ToLower().Contains("log ind"));

//            if (userLogin is null)
//            {
//                return;
//            }

//            userLogin.Submit();

//            const string btnTagName = "button";
//            const string btnNextText = "næste";
//            const string btnLogInText = "log ind";

//            const string usernameFormId = "username";
//            const string passwordFormId = "form-error";

//            var logInMethod = Configuration["AuthConfig:LogInMethod"];

//            // Act & Assert
//            IReadOnlyCollection<IWebElement> chooseLoginMethods = WebDriver.FindElements(By.TagName(btnTagName));
//            var loginMethodElement = chooseLoginMethods.FirstOrDefault(e => e.Text.ToLower().Contains(logInMethod));

//            if (loginMethodElement is null)
//            {
//                throw new NullReferenceException($"Login method was not found, using ({logInMethod})");
//            }

//            loginMethodElement.Submit();

//            WebDriver.FindElement(
//                By.Id(idToFind: usernameFormId))
//                ?.SendKeys(Configuration["AuthConfig:Credentials:Username"]);

//            IReadOnlyCollection<IWebElement> formSubmitButtons = WebDriver.FindElements(By.TagName(btnTagName));
//            formSubmitButtons.FirstOrDefault(e => e.Text.ToLower().Contains(btnNextText))?.Submit();

//            // Check for errors
//            try
//            {
//                var error = WebDriver.FindElement(By.Id("form-error-message-form-error"));

//                if (error is not null)
//                {
//                    throw new NullReferenceException(error.Text);
//                }
//            }
//            catch (NoSuchElementException)
//            {
//                // Ignore.
//            }

//            WebDriver.FindElement(
//                By.Id(idToFind: passwordFormId))
//                ?.SendKeys(Configuration["AuthConfig:Credentials:Password"]);

//            formSubmitButtons = WebDriver.FindElements(By.TagName(btnTagName));
//            formSubmitButtons.FirstOrDefault(e => e.Text.ToLower().Contains(btnLogInText))?.Submit();
//        }

//        private void NavigateTo(string url)
//        {
//            double timeout = double.Parse(Configuration["WebDriverConfig:TimeoutInSeconds"]);

//            WebDriver.Navigate().GoToUrl(new Uri(url));
//            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeout);
//            WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeout);
//        }

//        private void UploadFileToInputField()
//        {
//            bool fileHasBeenUploaded = false;
//            const string cssSelectInputTypeFile = "input[type='file']";
//            var cvConfig = Configuration.GetSection("AppConfig:Documents:CV");
//            string filePath = Path.Combine(cvConfig["Location"], cvConfig["FileName"]);

//            try
//            {
//                IWebElement inputField = WebDriver.FindElement(By.CssSelector(cssSelectInputTypeFile));
//                inputField?.SendKeys(filePath);

//                fileHasBeenUploaded = !fileHasBeenUploaded;
//            }
//            catch (NoSuchElementException)
//            {
//                Console.WriteLine("Element was either not found, or maximum amount of files has been reached for this type.");
//            }
//            catch (Exception)
//            {
//                Console.WriteLine("Unknown error occurred.");
//            }

//            if (fileHasBeenUploaded)
//            {
//                IWebElement saveBtn = WebDriver.FindElements(By.TagName("button"))
//                    .Where(btn => btn.Text.ToLower().Contains("gem"))
//                    .FirstOrDefault();

//                saveBtn?.Click();
//            }
//        }
//    }
//}
