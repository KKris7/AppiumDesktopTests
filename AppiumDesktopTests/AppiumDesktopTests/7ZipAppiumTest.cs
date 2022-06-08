using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace AppiumDesktopTests
{
    public class _7ZipAppiumTest
    {
        private const string AppiumServerUrl = "http://[::1]:4723/wd/hub";
        private const string App = @"C:\Program Files\7-Zip\7zFM.exe";

        private WindowsDriver<WindowsElement> driver;
        private WindowsDriver<WindowsElement> rootDriver;

        private string workDir;

        [OneTimeSetUp]
        public void Setup()
        {
            var desktopOptions = new AppiumOptions() { PlatformName = "Windows" };
            desktopOptions.AddAdditionalCapability("app", "Root");
            rootDriver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), desktopOptions);


            var options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", App);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServerUrl), options);

            workDir = Directory.GetCurrentDirectory() + @"\workdir";
            if (Directory.Exists(workDir))
            {
                Directory.Delete(workDir, true);
            }
            Directory.CreateDirectory(workDir);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
            rootDriver.Quit();
        }

        [Test]
        public void Test_7Zip_MaxCompression()
        {
            var locationTextBox = driver.FindElementByClassName("Edit");
            locationTextBox.SendKeys(@"C:\Program Files\7-Zip\" + Keys.Enter);

            var filesList = driver.FindElementByClassName("SysListView32");
            filesList.SendKeys(Keys.Control + 'a');
            driver.FindElementByName("Add").Click();

            string archiveFileName = workDir + "\\" + DateTime.Now.Ticks + ".7z";

            Thread.Sleep(500);

            var archiveView = rootDriver.FindElementByName("Add to Archive");
            var archiveNameTextBox = archiveView.FindElementByXPath("/Window/ComboBox/Edit[@Name='Archive:']");
            archiveNameTextBox.SendKeys(archiveFileName);

            var archiveFormat = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Archive format:']");
            archiveFormat.SendKeys(Keys.Home);

            var compressionLevel = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Compression level:']");
            compressionLevel.SendKeys(Keys.End);

            var compressionMethod = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Compression method:']");
            compressionMethod.SendKeys(Keys.Home);

            var dictionarySize = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Dictionary size:']");
            dictionarySize.SendKeys(Keys.End);

            var wordSize = archiveView.FindElementByXPath("/Window/ComboBox[@Name='Word size:']");
            wordSize.SendKeys(Keys.End);

            var okButton = archiveView.FindElementByXPath("/Window/Button[@Name='OK']");
            okButton.Click();
            Thread.Sleep(1000);

            // extract
            locationTextBox.SendKeys(archiveFileName + Keys.Enter);
            var extractButton = driver.FindElementByName("Extract");
            extractButton.Click();

            var okButtonExtract = driver.FindElementByName("OK");
            okButtonExtract.Click();

            // needed time
            Thread.Sleep(1500);

            string extracted = workDir + @"\7zFM.exe";
            FileAssert.AreEqual(App, extracted);
        }
    }
}
