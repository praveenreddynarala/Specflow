using Capgemini_Test_Project.BaseClasses;
using NUnit.Framework;
// Created by: Praveen Reddy Narala

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Capgemini_Test_Project.Page_objects
{
    /// <summary>
    /// As part of POM, class holds all Google Search Page elemnet locators
    /// Holds its page level actions
    /// </summary>
    public class GoogleSearchPage
    {
        #region variables
        private readonly IWebDriver _driverObj;
        private BrowserActions _browserActionsClassObj;
        private readonly ScenarioContext _scenarioContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor and it holds WebDriver object
        /// </summary>
        /// <param name="driverObj">WebDriver object</param>
        public GoogleSearchPage(IWebDriver driverObj, ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
            _driverObj = driverObj;
            _browserActionsClassObj = new BrowserActions(_driverObj, this._scenarioContext);
        }
        #endregion

        #region WebLocators from Google search page
        private readonly By bySearchTextFieldLocator = By.Name("q");
        private readonly By byGoogleSearchBtnLocator = By.XPath("//div[@class='VlcLAe']/descendant::input[@name='btnK']");
        private readonly By byAllAvailableLinksLocator = By.XPath("//h3/a");
        #endregion

        #region Page Level Action
        /// <summary>
        /// Method used to enter text in Google Search text field
        /// </summary>
        /// <param name="strTextObj">Text to enter</param>
        public void EnterTextInGoogleSearchTextField(string strTextObj)
        {
            _browserActionsClassObj.SetValue(bySearchTextFieldLocator, strTextObj);
        }

        /// <summary>
        /// Method used to click on Google Search button
        /// </summary>
        public void ClickOnGoogleSearchButton()
        {
            _browserActionsClassObj.Click(byGoogleSearchBtnLocator);
        }

        /// <summary>
        /// Method used to Count the number of links on result page
        /// </summary>
        /// <returns>Count of links</returns>
        public int GetCountOfReturnedLinks()
        {
            return _browserActionsClassObj.GetAllLinks(byAllAvailableLinksLocator).Count;
        }

        /// <summary>
        /// Method used to get specific link text
        /// </summary>
        /// <param name="iLinkIndex">Link index</param>
        /// <returns></returns>
        public string GetTextSpecificLink(int iLinkIndex)
        {
            return _browserActionsClassObj.GetAllLinks(byAllAvailableLinksLocator).ElementAt(iLinkIndex).Text;
        }
        #endregion
    }
}
