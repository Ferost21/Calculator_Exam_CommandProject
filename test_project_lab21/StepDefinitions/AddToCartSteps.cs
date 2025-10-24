using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SaucedemoTests.Pages;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.Threading;

[Binding]
public class AddToCartSteps
{
    private IWebDriver _driver;
    private LoginPage _loginPage;
    private ProductsPage _productsPage;

    [BeforeScenario]
    public void BeforeScenario()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddAdditionalOption("useAutomationExtension", false);

        var driverPath = new DriverManager().SetUpDriver(new ChromeConfig());
        _driver = new ChromeDriver(driverPath, options);
        _driver.Manage().Window.Maximize();
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _driver?.Quit();
    }

    [Given(@"the user manually logs in to the Sauce Demo site")]
    public void GivenTheUserManuallyLogsInToTheSauceDemoSite()
    {
        _loginPage = new LoginPage(_driver);
        _loginPage.NavigateTo("https://www.saucedemo.com/");
        Console.WriteLine("👉 Введіть логін і пароль вручну, натисніть 'Login'.");

        // Чекаємо, поки користувач залогіниться
        bool loggedIn = false;
        for (int i = 0; i < 30; i++) // максимум 30 секунд
        {
            Thread.Sleep(1000);
            if (_driver.Url.Contains("inventory.html")) // якщо відкрилась сторінка товарів
            {
                loggedIn = true;
                break;
            }
        }

        Assert.IsTrue(loggedIn, "Не вдалося виявити успішний вхід. Перевірте логін/пароль.");
        _productsPage = new ProductsPage(_driver);
        Console.WriteLine("✅ Вхід успішний! Продовжуємо тест...");
        Thread.Sleep(1000);
    }

    [When(@"the user adds the first item to the cart")]
    public void WhenTheUserAddsTheFirstItemToTheCart()
    {
        Thread.Sleep(1000);
        _productsPage.AddFirstItemToCart();
        Thread.Sleep(1500);
    }

    [Then(@"the shopping cart badge should show ""(.*)"" item and open the cart")]
    public void ThenTheShoppingCartBadgeShouldShowItemAndOpenTheCart(string expectedCount)
    {
        Thread.Sleep(1000);
        Assert.IsTrue(_productsPage.IsShoppingCartBadgeDisplayed(), "Shopping cart badge is not displayed.");

        var actualCount = _productsPage.GetShoppingCartItemCount();
        Assert.AreEqual(expectedCount, actualCount,
            $"Expected cart count to be {expectedCount}, but was {actualCount}");

        // 🛒 Відкриваємо кошик
        _productsPage.OpenCart();
        Thread.Sleep(1500);

        // ✅ Перевіряємо, що товар є в корзині
        Assert.IsTrue(_productsPage.IsItemInCart(), "The cart does not contain any items.");
        Console.WriteLine("🛍️ Товар успішно додано до кошика!");
    }
}
