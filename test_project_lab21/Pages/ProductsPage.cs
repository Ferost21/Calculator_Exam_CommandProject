using OpenQA.Selenium;

namespace SaucedemoTests.Pages
{
    public class ProductsPage
    {
        private readonly IWebDriver _driver;

        public ProductsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локатор першої кнопки "Add to cart" для тесту (за індексом або іншим критерієм)
        private IWebElement AddToCartButton => _driver.FindElement(By.ClassName("btn_inventory"));

        // Локатор іконки кошика (для перевірки)
        private IWebElement ShoppingCartBadge => _driver.FindElement(By.ClassName("shopping_cart_badge"));

        // Методи сторінки
        public void AddFirstItemToCart()
        {
            AddToCartButton.Click();
        }

        public bool IsShoppingCartBadgeDisplayed()
        {
            try
            {
                return ShoppingCartBadge.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetShoppingCartItemCount()
        {
            return ShoppingCartBadge.Text;
        }
    }
}