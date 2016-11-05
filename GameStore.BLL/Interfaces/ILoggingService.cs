using System;

namespace GameStore.BLL.Nlog
{
    public interface ILoggingService
    {
        void Info(string message);

        void Error(string message);

        void Error(Exception e);

        void Error(Exception e, string message);

        void Debug(string message);
    }
}