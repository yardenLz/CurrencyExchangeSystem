// CurrencyScraper.cs
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CurrencyExchangeSystem
{
    public class CurrencyScraper
    {
        private readonly Queue<string> _sourceUrls;

        public CurrencyScraper(IEnumerable<string> urls)
        {
            _sourceUrls = new Queue<string>(urls);
        }

        public CurrencyPair GetCurrencyPair(string pairName)
        {
            foreach (var url in _sourceUrls)
            {
                try
                {
                    return FetchCurrencyPair(url, pairName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Source {url} failed. Error: {ex.Message}");
                }
            }

            Console.WriteLine("All sources failed. Unable to fetch data.");
            return null;
        }

        private CurrencyPair FetchCurrencyPair(string url, string pairName)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");

            using (var driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);

                // Check if the page has fully loaded
                var js = (IJavaScriptExecutor)driver;
                if (!js.ExecuteScript("return document.readyState").Equals("complete"))
                {
                    throw new Exception("Page did not load completely.");
                }

                // Try to locate the element based on the URL
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement rateElement;
                if (url.Contains("yahoo.com"))
                {
                    rateElement = wait.Until(drv => drv.FindElement(By.XPath($"//td[contains(text(), '{pairName.Replace("/", "")}')]")));
                }
                else if (url.Contains("bloomberg.com"))
                {
                    rateElement = wait.Until(drv => drv.FindElement(By.XPath("//div[contains(@class, 'data-table-row-cell__value')]")));
                }
                else
                {
                    throw new Exception("Unsupported URL structure.");
                }

                var rateText = rateElement.Text.Trim();
                var rate = decimal.Parse(rateText, System.Globalization.CultureInfo.InvariantCulture);

                return new CurrencyPair
                {
                    PairName = pairName,
                    ExchangeRate = rate,
                    UpdateTime = DateTime.Now
                };
            }
        }
    }
}
