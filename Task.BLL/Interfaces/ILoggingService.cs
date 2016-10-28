using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.Nlog
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

