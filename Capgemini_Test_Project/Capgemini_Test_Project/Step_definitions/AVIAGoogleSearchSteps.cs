// Created by: Praveen Reddy Narala

using AventStack.ExtentReports;
using Capgemini_Test_Project.BaseClasses;
using Capgemini_Test_Project.Page_objects;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Capgemini_Test_Project
{
    /// <summary>
    /// Step definition file
    /// Holds all feature file steps
    /// </summary>
    [Binding]
    public class StepDefinitions
    {
        #region Variables
        private IWebDriver _driverObj;
        private GoogleSearchPage _googleSearchStepsClassObj;
        private readonly FeatureContext _featurContext = null;
        private readonly ScenarioContext _scenarioContext = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driverObj">WebDriver object</param>
        public StepDefinitions(IWebDriver driverObj, FeatureContext featurContext, ScenarioContext scenarioContext)
        {
            this._featurContext = featurContext;
            this._scenarioContext = scenarioContext;
            _driverObj = driverObj;
            _googleSearchStepsClassObj = new GoogleSearchPage(_driverObj, this._scenarioContext);
        }
        #endregion

        #region Step definitions
        [Given(@"user navigates to Goolgle home page")]
        public void GivenUserNavigatesToGoolgleHomePage()
        {
            _driverObj.Navigate().GoToUrl(FrameGlobals.strBaseUrl);
        }

        [When(@"user entered '(.*)' in the search text field")]
        public void WhenUserEnteredInTheSearchTextField(string strValue)
        {
            _googleSearchStepsClassObj.EnterTextInGoogleSearchTextField(strValue);
        }

        [When(@"clicks on Google Search button")]
        public void WhenClicksOnGoogleSearchButton()
        {
            _googleSearchStepsClassObj.ClickOnGoogleSearchButton();
        }

        [Then(@"verify the number of links returned on result page is : (.*)")]
        public void ThenVerifyTheNumberOfLinksReturnedOnResultPage(int iExpectedLinks)
        {
            int intActualLinkCount = _googleSearchStepsClassObj.GetCountOfReturnedLinks();
            //ScenarioContext.Current["LinkCount"] = intActualLinkCount;
            this._scenarioContext.Add("LinkCount", intActualLinkCount);
            Assert.That(intActualLinkCount == iExpectedLinks, "Actual links count is not same as expected links count");

        }

        [Then(@"print the linktext of the '(.*)' th link displayed related to Aviva Search")]
        public void ThenPrintTheLinktextOfThLinkDisplayedRelatedToAvivaSearch(int iLinkNo)
        {
            string fifthLink = _googleSearchStepsClassObj.GetTextSpecificLink(iLinkNo);
            //ScenarioContext.Current["FifthLink"] = fifthLink;
            this._scenarioContext.Add("FifthLink", fifthLink);
        }
        #endregion
    }
}
