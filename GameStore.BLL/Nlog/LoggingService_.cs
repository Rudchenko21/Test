using NLog;
using System;

namespace GameStore.BLL.Nlog
{
    public class LoggingService_ : Logger, ILoggingService
    {
        private Logger _logger;

        private readonly string _appDomain;

        public LoggingService_()
        {
            this._logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            _logger.Info( message);
        }

        public void Error(string message)
        {
            _logger.Error( "{message}");
        }

        public void Error(Exception e)
        {
            _logger.Error( "Message : {e.Message} TargetSite : {e.TargetSite} StackTrace : {e.StackTrace}");
        }

        public void Error(Exception e, string message)
        {
            _logger.Error( "Message : {message} TargetSite : {e.TargetSite} StackTrace : {e.StackTrace}");
        }

        public void Debug(string message)
        {
            _logger.Debug( "{message}");
        }
    }
}
