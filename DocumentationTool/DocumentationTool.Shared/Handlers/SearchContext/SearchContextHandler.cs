using DocumentationTool.Shared.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationTool.Shared.Handlers.SearchContext
{
    internal class SearchContextHandler
    {
        private readonly IWebDriver _webDriver;

        public SearchContextHandler(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void 

        private bool AddBookmarkForApprenticeshupByEducationSuffix(string suffix)
        {
            const string bookmarkedAttributeValue = "M17 3H7c-1.1 0-2 .9-2 2v16l7-3 7 3V5c0-1.1-.9-2-2-2z";
            int bookmarkCount = default;

            bool companyFound = FoundAnyCompanies();

            if (companyFound is false)
            {
                return companyFound;
            }

            // Find all companies with same education range.
            IReadOnlyCollection<IWebElement> categorisedCompanies =
                _webDriver.FindElements(By.CssSelector($"[class*='{"SoegOpslag_searchResultEntry"}']"));

            if (categorisedCompanies is null)
            {
                Console.WriteLine("Did not find any companies with specified name.");
                return false;
            }

            IEnumerable<IWebElement> bookmarkContainers =
                categorisedCompanies.Where(e => e.Text.Contains(suffix, StringComparison.OrdinalIgnoreCase));

            foreach (IWebElement container in bookmarkContainers)
            {
                IReadOnlyCollection<IWebElement> bookmarkSVGPaths = container.FindElements(By.TagName("path"));

                string pathAttributeValue = bookmarkSVGPaths.ElementAt(1).GetAttribute("d");

                if (pathAttributeValue.Equals(bookmarkedAttributeValue))
                {
                    Console.WriteLine($"This one is already bookmarked.");
                    continue;
                }

                IWebElement bookmark = container.FindElement(By.CssSelector($"[class*='AnsoegningslogBookmark_container']"));

                bookmark?.Click();
                bookmarkCount++;
            }

            Console.WriteLine("Bookmarked a total of [{0}] compan(y/ies).", bookmarkCount);

            return true;
        }

        private bool SearchApprenticeshipByCompany(string companyName)
        {
            const string url = "https://pms.xn--lrepladsen-d6a.dk";
            const string relativePath = "/soeg-opslag";

            const string inputFieldCssSelector = "input[name='fritekst']";

            bool navigatedToUrl = _webDriver.GoToUrl(url, relativePath);

            if (navigatedToUrl is false)
            {
                return navigatedToUrl;
            }

            try
            {
                IWebElement inputField = _webDriver.FindElement(By.CssSelector(inputFieldCssSelector));
                inputField?.SendKeys(companyName);

                IWebElement searchBtn = _webDriver.FindElements(By.TagName("button"))
                .FirstOrDefault(e => e.Text.Contains("søg", StringComparison.OrdinalIgnoreCase));

                searchBtn?.Click();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool FoundAnyCompanies()
        {
            try
            {
                const string alertCssSelector = "div[class*='Alert']";
                const string noResultsText = "ingen resultater";

                IWebElement alert = _webDriver.FindElement(By.CssSelector(alertCssSelector));

                if (alert is null)
                {
                    throw new NullReferenceException("Error occurred. But not specified.");
                }

                if (alert.Text.Contains(noResultsText, StringComparison.OrdinalIgnoreCase))
                {
                    throw new NotSupportedException("Entered company was not found. Try again.");
                }

                return false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }
    }
}
