using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace DissertationProject.Tests
{
    [TestFixture]
    public class SettingsTests
    {
        //Has to be created in the main program so that it can be accessed in different methods.
        IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            ChromeOptions ops = new ChromeOptions();
            //Tests run in the background.
            //ops.AddArgument("--headless");
            driver = new ChromeDriver(@"\Drivers\chromedriver_win32", ops);
        }


        [Test]
        public void editAllUserData()
        {
            // Navigate to the register page
            driver.Navigate().GoToUrl("https://dissertationdeployment.azurewebsites.net/Identity/Account/Login");

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
            Thread.Sleep(3000);

            // Go to the settings page
            IWebElement settings = driver.FindElement(By.Id("settingsbtn"));
            settings.Click();
            //Change the users details
            IWebElement fname = driver.FindElement(By.Id("fname"));
            fname.SendKeys("Joe");
            IWebElement sname = driver.FindElement(By.Id("sname"));
            sname.SendKeys("Smith");
            IWebElement jobName = driver.FindElement(By.Id("jobName"));
            jobName.SendKeys("Asda Worker");
            IWebElement income = driver.FindElement(By.Id("income"));
            income.SendKeys("30000");

            //Click submit
            IWebElement submit = driver.FindElement(By.Id("submitbtn"));
            submit.Click();

            //Assert

        }

    }
}
