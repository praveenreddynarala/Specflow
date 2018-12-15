// Created by: Praveen Reddy Narala

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini_Test_Project.BaseClasses
{
    /// <summary>
    /// This is a global configuration class
    /// Holds different enums
    /// Based on enums class will read the data from App.config file
    /// Reading the data from App.config file
    /// </summary>
    public class ConfigSectionHandler : IConfigurationSectionHandler
    {
        public ConfigSectionHandler()
            : base()
        {
        }

        public object Create(object oParent, object oConfigContext, System.Xml.XmlNode xmlSection)
        {
            return oParent;
        }
    }

    #region enums
    public enum FilePath
    {
        ScreenShot,
        Report,
        LogReport,
        TestData
    }
    public enum ResultStatus
    {
        Pass,
        Fail,
        Warning
    }
    public enum TestCompleteTime
    {
        TestCompletiomTime
    }
    internal enum BrowserTypes
    {
        Ie = 0,
        Ff = 1,
        Chrome = 2,
    }
    #endregion

    #region Static class
    /// <summary>
    /// Static class
    /// Used to read all configuration settings from App.config file
    /// </summary>
    public static class FrameGlobals
    {
        #region variables
        public static string strBaseUrl = null;
        //public static string strBrowserName = null;
        public static string strCapturescreenshot = null;
        public static DateTime dtStartedTime = DateTime.MinValue;
        internal static BrowserTypes enmBrowserType;
        //Used for Fiddler Integration
        private static Configuration _conFrameGlobals = null;
        //public static string strHtmlPath = null;
        public static string strImplicitWait = null;
        internal static BrowserTypes browserType;
        #endregion

        #region Reading Configuraion Data from App.config
        /// <summary>
        /// Static method used to read all configuration values from App.config file
        /// </summary>
        public static void Init()
        {
            try
            {
                ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
                DirectoryInfo currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                string constFileName = currentDirectory.FullName + "App.Config";
                ecf.ExeConfigFilename = constFileName;
                Configuration dllConfig = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
                _conFrameGlobals = dllConfig;
                dtStartedTime = DateTime.Now;
                dtStartedTime = DateTime.Now;
                strBaseUrl = dllConfig.AppSettings.Settings["Base_URL"].Value;
                enmBrowserType = (BrowserTypes)Enum.Parse(typeof(BrowserTypes), dllConfig.AppSettings.Settings["BrowserType"].Value.ToString(CultureInfo.InvariantCulture));
                strCapturescreenshot = dllConfig.AppSettings.Settings["CapturescreenshotforAllsteps"].Value;
                strImplicitWait = dllConfig.AppSettings.Settings["WaitTimeOut"].Value;
                browserType = (BrowserTypes)Enum.Parse(typeof(BrowserTypes), dllConfig.AppSettings.Settings["BrowserType"].Value.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Time out config
        /// <summary>
        /// Time Out from Configuration file.
        /// </summary>
        /// <param name="iTimeFromConfig">Key Name.</param>
        /// <returns>Timeout.</returns>
        public static uint TimeOutConfig(string iTimeFromConfig)
        {
            try
            {
                return uint.Parse(ConfigurationManager.AppSettings[iTimeFromConfig]);
            }
            catch (Exception)
            {
                return 60;
            }
        }
        #endregion

        #endregion
    }
}
