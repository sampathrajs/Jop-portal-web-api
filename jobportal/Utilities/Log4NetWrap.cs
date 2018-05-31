using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;

using log4net;
using log4net.Config;
 
namespace jobportal.Utilities
{
    public class Log4NetWrap
    {
        #region Private variables
        private ILog _log;
        private static bool configured;
        #endregion

        #region Constructor
        public Log4NetWrap(string name)
        {
            if (!configured)
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
                configured = true;
            }

            _log = LogManager.GetLogger(name);
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Writes error info to logger.
        /// </summary>
        /// <param name="e">message to log.</param>
        public void LogError(string message)
        {
            _log.Error(message);
        }
        /// <summary>
        /// Writes error info to logger.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        public void LogError(Exception exception)
        {
            _log.Error("Error: ", exception);
        }
        /// <summary>
        /// Writes error info to logger.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        /// /// <param name="req">Request to log.</param>
        public void LogError(string message, Exception exception)
        {
            _log.ErrorFormat("Error Message: {0}", message); 
            if (exception != null)
            {
                _log.Error("ERR: ", exception);
                _log.Error("Stack trace: " + exception.StackTrace);
            }
        }
       
        /// <summary>
        /// Writes error to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }
        /// <summary>
        /// Writes info to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogInfo(string message)
        {
            _log.Info(message);
        }
        /// <summary>
        /// Writes info to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogInfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }
        /// <summary>
        /// Writes info to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogWarn(string message)
        {
            _log.WarnFormat("WRN: \n{0}", message);
        }
        /// <summary>
        /// Writes info to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogWarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format,args);
        }
        /// <summary>
        /// Writes debug info to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogDebug(string message)
        {
            _log.Debug(message);
        }
        /// <summary>
        /// Writes Debug to logger.
        /// </summary>
        /// <param name="e">String to log.</param>
        public void LogDebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        #endregion
    }
}
