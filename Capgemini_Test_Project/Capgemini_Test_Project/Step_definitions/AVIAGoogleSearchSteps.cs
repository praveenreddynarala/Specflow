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
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driverObj">WebDriver object</param>
        public StepDefinitions(IWebDriver driverObj)
        {
            _driverObj = driverObj;
            _googleSearchStepsClassObj = new GoogleSearchPage(_driverObj);
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

        [Then(@"user click on Google Search button")]
        public void ThenUserClickOnGoogleSearchButton()
        {
            _googleSearchStepsClassObj.ClickOnGoogleSearchButton();
        }

        [Then(@"verify the number of links returned on result page is : (.*)")]
        public void ThenVerifyTheNumberOfLinksReturnedOnResultPage(int iExpectedLinks)
        {
            int intActualLinkCount = _googleSearchStepsClassObj.GetCountOfReturnedLinks();                             
            ScenarioContext.Current["LinkCount"] = intActualLinkCount;
            //Assert.IsTrue(intActualLinkCount == iExpectedLinks, "Actual link cound is not equals with expected links count");
            Assert.That(intActualLinkCount == iExpectedLinks, "Actual links count is not same as expected links count");

        }

        [Then(@"print the linktext of the '(.*)' th link displayed related to Aviva Search")]
        public void ThenPrintTheLinktextOfThLinkDisplayedRelatedToAvivaSearch(int iLinkNo)
        {
            string fifthLink = _googleSearchStepsClassObj.GetTextSpecificLink(iLinkNo);
            ScenarioContext.Current["FifthLink"] = fifthLink;
        }
        #endregion
    }
}
