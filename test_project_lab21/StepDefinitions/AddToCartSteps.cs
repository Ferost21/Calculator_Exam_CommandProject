using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SaucedemoTests.Pages;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;

[Binding]
public class AddToCartSteps
{
    private IWebDriver _driver;
    private LoginPage _loginPage;
    private ProductsPage _productsPage;

    [BeforeScenario]
    public void BeforeScenario()
    {
        // 1 Завантажуємо драйвер через WebDriverManager
        new DriverManager().SetUpDriver(new ChromeConfig());

        // 2 Створюємо ChromeOptions і ВИМКНУМО Selenium Manager
        var options = new ChromeOptions();

        // Цей прапорець вимикає пошук selenium-manager.exe
        options.AddAdditionalOption("useAutomationExtension", false);

        // (Опціонально) відкривати одразу на весь екран
        options.AddArgument("--start-maximized");

        // 3 Вказуємо шлях до драйвера явно, щоб Selenium не шукав selenium-manager.exe
        var driverPath = new DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver(driverPath, options);

        _driver.Manage().Window.Maximize();
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _driver?.Quit();
    }

    [Given(@"the user is on the Sauce Demo login page")]
    public void GivenTheUserIsOnTheSauceDemoLoginPage()
    {
        _loginPage = new LoginPage(_driver);
        _loginPage.NavigateTo("https://www.saucedemo.com/");
    }

    [When(@"the user logs in with username ""(.*)"" and password ""(.*)""")]
    public void WhenTheUserLogsIn(string username, string password)
    {
        _loginPage.EnterCredentials(username, password);
        _loginPage.ClickLogin();
        _productsPage = new ProductsPage(_driver);
    }

    [When(@"the user adds the first item to the cart")]
    public void WhenTheUserAddsTheFirstItemToTheCart()
    {
        _productsPage.AddFirstItemToCart();
    }

    [Then(@"the shopping cart badge should show ""(.*)"" item")]
    public void ThenTheShoppingCartBadgeShouldShowItem(string expectedCount)
    {
        Assert.IsTrue(_productsPage.IsShoppingCartBadgeDisplayed(), "Shopping cart badge is not displayed.");
        Assert.AreEqual(expectedCount, _productsPage.GetShoppingCartItemCount(),
            $"Expected cart count to be {expectedCount}, but was {_productsPage.GetShoppingCartItemCount()}");
    }
}
