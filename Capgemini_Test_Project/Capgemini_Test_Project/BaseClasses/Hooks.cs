// Created By : Praveen Reddy Narala

using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using Capgemini_Test_Project.BaseClasses;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Capgemini_Test_Project.BaseClasses
{
    /// <summary>
    /// This class is the main class in our project
    /// Holding 'BeforeTestRun', 'AfterTestRun', 'BeforeScenario', 'AfterScenario', 'BeforeFeature', 'AfterTest' attributes
    /// Holding WebDriver initialization
    /// Holding Extent Report generation
    /// </summary>
    [Binding]
    public class Hooks
    {
        #region variables
        private static ExtentTest extFeatureName;
        private static ExtentTest extScenario;
        public static ExtentTest extLogger;
        private static ExtentReports extReports;

        private readonly IObjectContainer _objContainer;
        private IWebDriver _iWebDriver;
        

        public static object FileReader { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objContainer"></param>
        public Hooks(IObjectContainer objContainer)
        {
            _objContainer = objContainer;
        }
        #endregion

        #region Extent Reports
        /// <summary>
        /// Method used to Initialize the extent report before test starts
        /// </summary>
        [BeforeTestRun]
        public static void InitializeReports()
        {
            //Initialize extent report before test starts
            var vHTMLReporter = new ExtentHtmlReporter(@"D:\data\Personal\Capgemini_Test_Project\Capgemini_Test_Project\Capgemini_Test_Project\Test_output\Reports\ExtentReport.html");
            vHTMLReporter.Configuration().Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;

            //Attach report to reporter
            extReports = new ExtentReports();

            extReports.AttachReporter(vHTMLReporter);
        }

        /// <summary>
        /// Method used to flush the extent report after test run
        /// </summary>
        [AfterTestRun]
        public static void TearDownReports()
        {
            extReports.Flush();
        }

        /// <summary>
        /// Method used to create ExtentTest Feature object
        /// </summary>
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            extFeatureName = extReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        /// <summary>
        /// Method used to generate generic Extent Report using Scenarios
        /// Used reflections to generate report for all pending step definitions
        /// </summary>
        /// <param name="iDriver">WebDriver Object</param>
        [AfterStep]
        public static void InsertReportingSteps(IWebDriver iDriver, ScenarioContext scenarioContext)
        {
            var vStepType = scenarioContext.CurrentScenarioBlock.ToString();

            try
            {
               
                PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
                MethodInfo mGetter = pInfo.GetGetMethod(nonPublic: true);
                object oTestResult = mGetter.Invoke(scenarioContext, null);


                if (scenarioContext.TestError == null) //if test does not return any error while execution
                {
                    if (vStepType == "Given")
                    {
                        extScenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
                    }
                    else if (vStepType == "When")
                    {
                        extScenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
                    }
                    else if (vStepType == "Then")
                    {
                        
                        //Appending returned values to report logs
                        if (scenarioContext.ContainsKey("LinkCount"))
                        {
                            int count = Convert.ToInt16(scenarioContext["LinkCount"]);
                            extScenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text + " : <br/> Actual Result is : <b>" + count + "</b>");
                            scenarioContext.Remove("LinkCount");
                        }
                        else if (scenarioContext.ContainsKey("FifthLink"))
                        {
                            string fifthLink = Convert.ToString(scenarioContext["FifthLink"]);
                            extScenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text + " : " + fifthLink);
                            scenarioContext.Remove("FifthLink");
                        }
                        else
                        {
                            extScenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
                        }
                        
                    }
                    else if (vStepType == "And")
                    {
                        
                        extScenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text);
                    }
                    
                }
                else if (scenarioContext.TestError != null) //if test returns error during the execution
                {
                    if (vStepType == "Given")
                    {
                        extScenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.InnerException);
                    }
                    else if (vStepType == "When")
                    {
                        extScenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.InnerException).AddScreenCaptureFromPath(CaptureScreenshot(iDriver)); ;
                    }
                    else if (vStepType == "Then")
                    {
                        if (scenarioContext.ContainsKey("Exception"))
                        {
                            string strExcMsg = scenarioContext["Exception"].ToString(); //Exception logging in reports
                            extScenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text +" : "+ strExcMsg).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CaptureScreenshot(iDriver));
                            scenarioContext.Remove("Exception");
                        }
                        else
                        {
                            extScenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CaptureScreenshot(iDriver)); ;
                        }
                    }
                    else if (vStepType == "And")
                    {
                        extScenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Fail(scenarioContext.TestError.Message).AddScreenCaptureFromPath(CaptureScreenshot(iDriver)); ;
                    }
                }

                //Pending Status
                if (oTestResult.ToString() == "StepDefinitionPending")
                {
                    if (vStepType == "Given")
                    {
                        extScenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                    }
                    else if (vStepType == "When")
                    {
                        extScenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                    }
                    else if (vStepType == "Then")
                    {
                        extScenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                    }
                    else if (vStepType == "And")
                    {
                        extScenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                    }
                }
            }
            catch(Exception ex)
            {
                scenarioContext["Exception"] = ex.Message;
            }
        }
        #endregion

        #region WebDriver initialization
        /// <summary>
        /// Method used to initialize WebDriver
        /// Storing WebDriver object in Object Container for maintaining state of the object in all child classes
        /// Added ImplicitWait
        /// </summary>
        [BeforeScenario]
        public void InitializeDriver(ScenarioContext scenarioContext)
        {
            try
            {
                FrameGlobals.Init();
                if (FrameGlobals.browserType == BrowserTypes.Chrome)
                {
                    ChromeOptions chromeCapabilities = new ChromeOptions();
                    var arr = new string[6] { "--start-maximized", "--ignore-certificate-errors", "--disable-popup-blocking", "--disable-default-apps", "--auto-launch-at-startup", "--always-authorize-plugins" };
                    chromeCapabilities.AddArguments(arr);
                    _iWebDriver = new ChromeDriver(chromeCapabilities);
                }
                else if (FrameGlobals.browserType == BrowserTypes.Ie)
                {
                    //Deleting cookies in ie browser through command line.
                    var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2");
                    var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
                    proc.Start();
                    var options = new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        RequireWindowFocus = true,
                        IgnoreZoomLevel = true,
                        EnableNativeEvents = true
                    };
                    _iWebDriver = new InternetExplorerDriver(options);
                }
            }catch(Exception ex)
            {
                //ScenarioContext.Current["Exception"] = ex.Message;
                scenarioContext.Add("Exception", ex.Message);
            }
            _iWebDriver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(Double.Parse(FrameGlobals.strImplicitWait)));
            _objContainer.RegisterInstanceAs<IWebDriver>(_iWebDriver);
            extScenario = extFeatureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        /// <summary>
        /// Method used to perform quit WebDriver
        /// </summary>
        [AfterScenario]
        public void CloseDriver()
        {
            _iWebDriver?.Quit();
        }
        #endregion

        #region Capture Screenshot
        /// <summary>
        /// Take Screenshot
        /// </summary>
        /// <param name="iDriver">WebDriver object</param>
        /// <returns></returns>
        public static string CaptureScreenshot(IWebDriver iDriver)
        {
            string strPathProject = null;
            string strPathScreen = null;
            string strFileName = null;
            string strDirectoryPath = null;
            string strFilePath = null;
            string strScreenShotsPath = null;
            try
            {
                strPathProject = AppDomain.CurrentDomain.BaseDirectory;
                strPathScreen = strPathProject.Replace("\\bin\\Debug", "");
                strScreenShotsPath = strPathScreen + "Test_output\\Screenshots\\";

                if (FrameGlobals.browserType == BrowserTypes.Ff)
                {
                    strDirectoryPath = strScreenShotsPath + "FF";
                }
                //IE
                else if (FrameGlobals.browserType == BrowserTypes.Ie)
                {
                    strDirectoryPath = strScreenShotsPath + "IE";
                }
                //Chrome    
                else if (FrameGlobals.browserType == BrowserTypes.Chrome)
                {
                    strDirectoryPath = strScreenShotsPath + "Chrome";
                }

                if (!Directory.Exists(strDirectoryPath))
                {
                    Directory.CreateDirectory(strDirectoryPath);
                }

                strFileName = DateTime.Now.Day.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Month.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Year.ToString(CultureInfo.CurrentCulture) + "_" + DateTime.Now.Hour.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Minute.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Hour.ToString(CultureInfo.CurrentCulture) + DateTime.Now.Second.ToString(CultureInfo.CurrentCulture) + ".Png";
                strFilePath = strDirectoryPath + "\\" + strFileName;
                Screenshot snapShot = ((ITakesScreenshot)iDriver).GetScreenshot();
                snapShot.SaveAsFile(strFilePath, ScreenshotImageFormat.Png);

            }
            catch (Exception ex)
            {
                ScenarioContext.Current["Exception"] = ex.Message;
            }
            return strFilePath;
        }
        #endregion

    }
}
