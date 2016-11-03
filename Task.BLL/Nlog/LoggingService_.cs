using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.Nlog
{
    public class LoggingService_ : Logger, ILoggingService
    {
        private Logger logger;
        private readonly string appDomain;
        public LoggingService_()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }
        public void Info(string message)
        {
            logger.Info( message);
        }
        public void Error(string message)
        {
            logger.Error( $"{message}");
        }
        public void Error(Exception e)
        {
            logger.Error( $"Message : {e.Message} TargetSite : {e.TargetSite} StackTrace : {e.StackTrace}");
        }
        public void Error(Exception e, string message)
        {
            logger.Error( $"Message : {message} TargetSite : {e.TargetSite} StackTrace : {e.StackTrace}");
        }
        public void Debug(string message)
        {
            logger.Debug( $"{message}");
        }
    }
}
