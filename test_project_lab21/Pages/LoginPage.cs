using OpenQA.Selenium;

namespace SaucedemoTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        // Конструктор, який приймає екземпляр драйвера
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локатори елементів
        private IWebElement UsernameField => _driver.FindElement(By.Id("user-name"));
        private IWebElement PasswordField => _driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => _driver.FindElement(By.Id("login-button"));

        // Методи сторінки
        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void EnterCredentials(string username, string password)
        {
            UsernameField.SendKeys(username);
            PasswordField.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }
    }
}