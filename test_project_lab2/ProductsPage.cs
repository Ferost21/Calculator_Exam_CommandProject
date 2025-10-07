using OpenQA.Selenium;

namespace SauceDemoTests.Pages
{
    public class ProductsPage
    {
        private readonly IWebDriver driver;
        public ProductsPage(IWebDriver driver) => this.driver = driver;

        public IWebElement AddToCartButton => driver.FindElement(By.CssSelector("button[data-test='add-to-cart-sauce-labs-backpack']"));
        public IWebElement CartBadge => driver.FindElement(By.ClassName("shopping_cart_badge"));

        public void AddToCart() => AddToCartButton.Click();
    }
}