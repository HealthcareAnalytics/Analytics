using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace AutomationTests
{
    [TestClass]
    public class UnitTest1
    {
        static IWebDriver driverFF;

        [AssemblyInitialize]
        public static void SetUp(TestContext contex)
        {
            driverFF = new FirefoxDriver();
        }

        [TestMethod]
        public void HomeIndexView()
        {
            driverFF.Navigate().GoToUrl(this.GetAbsoluteUrl("/Home"));
            Assert.IsTrue(driverFF.FindElement(By.Id("registerLink")).Displayed);
        }



        [TestMethod]
        public void EmployeeIndexView()
        {
            driverFF.Navigate().GoToUrl(this.GetAbsoluteUrl("/Employees"));
            Assert.IsTrue(driverFF.FindElement(By.Id("registerLink")).Displayed);
        }

        [TestMethod]
        public void EmployeeCreateView()
        {
            driverFF.Navigate().GoToUrl(this.GetAbsoluteUrl("/Employees/create"));
            Assert.IsTrue(driverFF.FindElement(By.Id("NameDetailsId")).Displayed);
        }


        [TestMethod]
        public void PatientsIndexView()
        {
            driverFF.Navigate().GoToUrl(this.GetAbsoluteUrl("/Patients"));
            Assert.IsTrue(driverFF.FindElement(By.Id("registerLink")).Displayed);
        }

        [TestMethod]
        public void PatientsCreateView()
        {
            driverFF.Navigate().GoToUrl(this.GetAbsoluteUrl("/Patients/create"));
            Assert.IsTrue(driverFF.FindElement(By.Id("NameDetailsId")).Displayed);
        }

        public string GetAbsoluteUrl(string relativeUrl)
        {
            const int iisPort = 8482;
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return String.Format("http://localhost:{0}{1}", iisPort, relativeUrl);
        }
    }


}
