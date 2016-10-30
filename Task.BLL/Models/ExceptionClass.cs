using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.Models
{
    public class ExceptionClass
    {
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string TargetSite { get; set; }
        public int StatusCode { get; set; }
        public string Assembly { get; set; }
        public string AppDomain { get; set; }
        public override string ToString()
        {
            return "StatusCode : {StatusCode} Message : {Message}  Assembly: {Assembly}  AppDomain : {AppDomain}  TargetSite : {TargetSite}   StackTrace : {StackTrace}";
        }
    }
}
