using System;
using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UnitTest
{
    public class KrakenTest
    {
        public IWebDriver _webDriver;
        public WebDriverWait _webDriverWait;

        private const string ExpectedFavoritedMarketName = "ETH/USD";

        [SetUp]
        public void Setup()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Manage().Window.Size = new Size(1920, 1080);
            _webDriver.Navigate().GoToUrl("https://demo-futures.kraken.com/futures/PI_ETHUSD");

            _webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(15));

            var tryDemoButton = _webDriver.FindElement(By.CssSelector("body > app-root > mat-sidenav-container > mat-sidenav-content > main > app-toolbar > section > div.main > button.mat-focus-indicator.signup.mat-button.mat-button-base.global__text.global__text-body.ng-star-inserted"));
            tryDemoButton.Click();

            var continueButton = _webDriver.FindElement(By.CssSelector("#mat-dialog-0 > demo-credentials > pro-dialog > mat-card > mat-card-content > div > form > button"));
            continueButton.Click();

            _webDriverWait.Until(
            ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("/html/body/app-root/mat-sidenav-container")));
        }
        [Test]
        public void AddMarketToFavoriteTest()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            var openMarket = _webDriver.FindElement(By.CssSelector("body > app-root > mat-sidenav-container > mat-sidenav-content > main > app-toolbar > section > div.main > section > market-dropdowns > div.desktop-dropdown > button"));
            openMarket.Click();


            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            var selectPair = _webDriver.FindElement(By.CssSelector("#cdk-overlay-7 > div > div > market-picker > section > div.assets.ng-star-inserted > market-picker-asset:nth-child(3) > div.icon > market-icon > img"));
            new Actions(_webDriver).MoveToElement(selectPair).Perform();

            var addToFavoritesButton = _webDriver.FindElement(By.CssSelector("#cdk-overlay-7 > div > div > market-picker > section > cdk-virtual-scroll-viewport > div.cdk-virtual-scroll-content-wrapper > market-picker-ticker:nth-child(2) > section > div.icon.global__text.global__text-body"));
            new Actions(_webDriver).MoveToElement(addToFavoritesButton).Perform();
            
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            var favoriteMarketsButton = _webDriver.FindElement(By.CssSelector("#cdk-overlay-7 > div > div > market-picker > section > cdk-virtual-scroll-viewport > div.cdk-virtual-scroll-content-wrapper > market-picker-ticker:nth-child(2) > section > div.icon.global__text.global__text-body > fa-icon > svg"));
            favoriteMarketsButton.Click();

            String actualFavoritedMarketName = _webDriver.FindElements(By.CssSelector("#cdk-overlay-19 > div > div > market-picker > cdk-virtual-scroll-viewport > div.cdk-virtual-scroll-content-wrapper > market-picker-ticker:nth-child(1) > div.maturity.global__text.global__text-subheading"))[1].Text;
            Assert.AreEqual(ExpectedFavoritedMarketName, actualFavoritedMarketName);

        }

        [TearDown]
        public void TearDown()
        {
            var deleteFromFavoriteButton = _webDriver.FindElement(By.CssSelector("#cdk-overlay-3 > div > div > market-picker > cdk-virtual-scroll-viewport > div.cdk-virtual-scroll-content-wrapper > market-picker-ticker:nth-child(1) > div.icon.global__text.global__text-body"));
            deleteFromFavoriteButton.Click();

            _webDriver.Quit();
        }
    }
}