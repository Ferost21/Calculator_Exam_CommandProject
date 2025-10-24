using OpenQA.Selenium;
using System.Threading;

namespace SaucedemoTests.Pages
{
    public class ProductsPage
    {
        private readonly IWebDriver _driver;

        public ProductsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локатор першої кнопки "Add to cart"
        private IWebElement AddToCartButton => _driver.FindElement(By.ClassName("btn_inventory"));

        // Іконка кошика
        private IWebElement ShoppingCartIcon => _driver.FindElement(By.ClassName("shopping_cart_link"));

        // Бейдж кількості товарів у кошику
        private IWebElement ShoppingCartBadge => _driver.FindElement(By.ClassName("shopping_cart_badge"));

        // Елемент товару в корзині (на сторінці корзини)
        private IWebElement CartItem => _driver.FindElement(By.ClassName("cart_item"));

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

        public void OpenCart()
        {
            ShoppingCartIcon.Click();
            Thread.Sleep(1500); // 🕒 зачекати поки сторінка корзини відкриється
        }

        public bool IsItemInCart()
        {
            try
            {
                return CartItem.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
