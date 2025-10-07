using OpenQA.Selenium;

namespace SauceDemoTests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        public LoginPage(IWebDriver driver) => this.driver = driver;

        public IWebElement Username => driver.FindElement(By.Id("user-name"));
        public IWebElement Password => driver.FindElement(By.Id("password"));
        public IWebElement LoginButton => driver.FindElement(By.Id("login-button"));

        public void Login(string username, string password)
        {
            Username.SendKeys(username);
            Password.SendKeys(password);
            LoginButton.Click();
        }
    }
}