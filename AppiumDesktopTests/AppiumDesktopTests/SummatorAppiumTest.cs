using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace AppiumDesktopTests
{
    public class SummatorAppiumTest
    {
        public const string AppiumServerUri = "http://[::1]:4723/wd/hub";
        public const string SummatorAppPath = @"D:\SummatorDesktopApp.exe";
        private WindowsDriver<WindowsElement> driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Windows" };
            appiumOptions.AddAdditionalCapability("app", SummatorAppPath);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUri), appiumOptions);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void ValidTest()
        {
            var firstNumField = driver.FindElementByAccessibilityId("textBoxFirstNum");
            var secondNumField = driver.FindElementByAccessibilityId("textBoxSecondNum");
            var resultField = driver.FindElementByAccessibilityId("textBoxSum");
            var calculateButton = driver.FindElementByAccessibilityId("buttonCalc");

            firstNumField.Clear();
            secondNumField.Clear();

            firstNumField.Click();
            firstNumField.SendKeys("12");
            secondNumField.Click();
            secondNumField.SendKeys("30");

            calculateButton.Click();

            var expectedResult = "42";

            Assert.That(expectedResult, Is.EqualTo(resultField.Text));

        }
        [Test]
        public void InvalidTest()
        {
            var firstNumField = driver.FindElementByAccessibilityId("textBoxFirstNum");
            var secondNumField = driver.FindElementByAccessibilityId("textBoxSecondNum");
            var resultField = driver.FindElementByAccessibilityId("textBoxSum");
            var calculateButton = driver.FindElementByAccessibilityId("buttonCalc");

            firstNumField.Clear();
            secondNumField.Clear();

            firstNumField.Click();
            firstNumField.SendKeys("");
            secondNumField.Click();
            secondNumField.SendKeys("");

            calculateButton.Click();

            var expectedResult = "error";

            Assert.That(expectedResult, Is.EqualTo(resultField.Text));

        }
    }
}