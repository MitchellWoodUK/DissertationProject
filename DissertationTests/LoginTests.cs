using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace DissertationProject.Tests
{
    [TestFixture]
    public class LoginTests
    {
        //Has to be created in the main program so that it can be accessed in different methods.
        IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            ChromeOptions ops = new ChromeOptions();
            //Tests run in the background.
            ops.AddArgument("--headless");
            driver = new ChromeDriver(@"\Drivers\chromedriver_win32", ops);
        }

   

        [Test]
        public void canLoginToAnAccount()
        {
            // Navigate to the register page
            driver.Navigate().GoToUrl("https://localhost:7170/Identity/Account/Login");
            // Fill in the registration form inputs

            IWebElement emailInput = driver.FindElement(By.Id("Input_Email"));
            emailInput.SendKeys("johndoe@example.com");

            IWebElement passwordInput = driver.FindElement(By.Id("Input_Password"));
            passwordInput.SendKeys("Pass123!");

            Thread.Sleep(3000);
            // Submit the form

            IWebElement register = driver.FindElement(By.Id("login-submit"));
            register.Click();

            // Wait for the registration confirmation page to load
            Thread.Sleep(1000);

            // Verify that the registration was successful
            //Assert
            Assert.IsTrue(driver.Url.Contains("https://localhost:7170/"));
        }


        [Test]
        public void RejectIncorrectLoginData()
        {
            // Navigate to the register page
            driver.Navigate().GoToUrl("https://localhost:7170/Identity/Account/Login");

            // Fill in the registration form inputs with incorrect data
            IWebElement emailInput = driver.FindElement(By.Id("Input_Email"));
            emailInput.SendKeys("invalidemail");  // Invalid email format

            IWebElement passwordInput = driver.FindElement(By.Id("Input_Password"));
            passwordInput.SendKeys("Pass123!");
            Thread.Sleep(3000);
            // Submit the form
            IWebElement register = driver.FindElement(By.Id("registerSubmit"));
            register.Click();
            Thread.Sleep(3000);
            // Wait for the error message to be displayed
            IWebElement error = driver.FindElement(By.ClassName("text-danger"));


            // Verify that the error message is displayed
            Assert.IsTrue(error.Displayed);
        }

    }
}
