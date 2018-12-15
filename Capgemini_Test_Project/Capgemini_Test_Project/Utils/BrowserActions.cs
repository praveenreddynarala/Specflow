// Created by: Praveen Reddy Narala

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Capgemini_Test_Project.BaseClasses
{
    /// <summary>
    /// Class holds common actions relats to WebDriver
    /// Capture Screenshot
    /// Exception Handling
    /// </summary>
    public class BrowserActions
    {
        #region variables
        private IWebDriver _iDriver;
        private IWebElement _iWebElement;
        private IList<IWebElement> _iWebElementList;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor holds the object of WebDriver
        /// </summary>
        /// <param name="driverObj">WebDriver object</param>
        public BrowserActions(IWebDriver iDriver)
        {
            _iDriver = iDriver;
        }
        #endregion

        #region Wait
        /// <summary>
        /// Implicit Wait for specific duration in Seconds.
        /// </summary>
        /// <param name="iTime">Timeout duration in Seconds.</param>
        /// <returns>Returns true if operation is successfully done otherwise false</returns>
        public WebDriverWait DriverWait(uint iTime = 0)
        {
            if (iTime == 0)
                iTime = FrameGlobals.TimeOutConfig(FrameGlobals.strImplicitWait);
            return new WebDriverWait(_iDriver, TimeSpan.FromSeconds(iTime));
        }

        /// <summary>
        /// Wait for the Element to exists before assignment.
        /// </summary>
        /// <param name="byObj">Locator Identification</param>
        private bool WaitAndGetElement(By bBy)
        {
            bool result = true;
            var elementInfocus = DriverWait(uint.Parse(FrameGlobals.strImplicitWait)).Until(d =>
            {
                try
                {
                    _iWebElement = _iDriver.FindElement(bBy);

                    if (!_iWebElement.Displayed)
                    {
                        result = false;
                    }
                }
                catch (ElementNotVisibleException)
                {
                    _iWebElement = DriverWait().Until(driver => driver.FindElement(bBy));
                    if (!_iWebElement.Displayed)
                    {
                        Hooks.CaptureScreenshot(_iDriver);
                        ScenarioContext.Current["Exception"] = "Element is not visisble. Please check element location and try run test again.";
                    }
                }
                catch(Exception ex)
                {
                    Hooks.CaptureScreenshot(_iDriver);
                    ScenarioContext.Current["Exception"] = ex.Message;
                }
                return _iWebElement;
            });
            return result;
        }

        /// <summary>
        /// Wait for multiple elements and returns true If all elements are found otherwise false 
        /// </summary>
        /// <param name="byObj">Element locator</param>
        /// <returns>Returns true If all elements are found otherwise false</returns>
        protected bool WaitAndGetElements(By bBy)
        {
            bool bGetElement = false;
            try
            {
                _iWebElementList = _iDriver.FindElements(bBy);
                foreach (var element in _iWebElementList)
                {
                    if (!(DriverWait(uint.Parse(FrameGlobals.strImplicitWait)).Until(d => element).Enabled))
                    {
                        Hooks.CaptureScreenshot(_iDriver);
                        ScenarioContext.Current["Exception"] = "Time out exception. Please try run test case again.";
                    }
                }
                bGetElement = true;
            }catch(Exception ex)
            {
                Hooks.CaptureScreenshot(_iDriver);
                ScenarioContext.Current["Exception"] = ex.Message;
            }
            return bGetElement;
        }
        #endregion        

        #region Actions
        /// <summary>
        /// Method used to click on WebElement
        /// </summary>
        /// <param name="byObj">By object</param>
        public void Click(By bBy)
        {
            WaitAndGetElement(bBy);
            try
            {
                _iWebElement.Click();
            }catch (StaleElementReferenceException)
            {
                _iDriver.FindElement(bBy).Click();
            }catch(Exception ex)
            {
                Hooks.CaptureScreenshot(_iDriver);
                ScenarioContext.Current["Exception"] = ex.Message;
            }
        }

        /// <summary>
        /// Methods used to set/enter value in WebElements/Text fields
        /// </summary>
        /// <param name="bBy">By object</param>
        /// <param name="strText">Text</param>
        public void SetValue(By bBy, string strText)
        {
            WaitAndGetElement(bBy);
            _iWebElement.SendKeys(strText);
        }

        /// <summary>
        /// Method used to return IList of Webelements
        /// </summary>
        /// <param name="bBy">By object</param>
        /// <returns></returns>
        public IList<IWebElement> GetAllLinks(By bBy)
        {
            WaitAndGetElements(bBy);
            return _iWebElementList;
        }
        #endregion
    }
}
