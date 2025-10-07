using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SauceDemoTests.Pages;
using TechTalk.SpecFlow;
using NUnit.Framework; // або using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SauceDemoTests.Steps
{
    [Binding]
    public class AddToCartSteps
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private ProductsPage productsPage;

        [BeforeScenario]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }

        [Given(@"I am on the SauceDemo login page")]
        public void GivenIAmOnTheSauceDemoLoginPage()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            loginPage = new LoginPage(driver);
        }

        [When(@"I login as ""(.*)"" with password ""(.*)""")]
        public void WhenILoginAsWithPassword(string username, string password)
        {
            loginPage.Login(username, password);
            productsPage = new ProductsPage(driver);
        }

        [When(@"I add the ""(.*)"" to the cart")]
        public void WhenIAddTheToTheCart(string product)
        {
            productsPage.AddToCart();
        }

        [Then(@"the cart badge should show ""(.*)""")]
        public void ThenTheCartBadgeShouldShow(string count)
        {
            Assert.AreEqual(count, productsPage.CartBadge.Text);
        }
    }
}