using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Twinstar
{
    class FindBy
    {
        private static WebDriverWait _wait => new(MainWindow.Driver, TimeSpan.FromSeconds(10));

        private readonly By _by;

        public FindBy(By by)
        {
            _by = by;
        }

        public IWebElement GetElement()
        {
            try
            {
                return MainWindow.Driver.FindElement(_by);
            }
            catch
            {
                return null;
            }
        }

        public IWebElement WaitForElementToBeVisible()
        {
            IWebElement element;

            return _wait.Until(driver =>
            {
                element = GetElement();
                return (element?.Displayed == true) ? element : null;
            });
        }

        public IWebElement WaitForElementToBeClickable()
        {
            IWebElement element;

            return _wait.Until(driver =>
            {
                element = GetElement();
                return (element?.Displayed == true && element?.Enabled == true) ? element : null;
            });
        }
    }
}
