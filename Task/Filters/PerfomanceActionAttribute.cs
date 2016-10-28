using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task.BLL.Nlog;

namespace Task.Filters
{
    public class PerfomanceActionAttribute: System.Web.Mvc.FilterAttribute, IActionFilter
    {
        
        private Stopwatch timer;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            timer = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILoggingService>();
            timer.Stop();
            if (filterContext.Exception == null)
            {
                logger.Info($"Controller: {filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}  ActionName : {filterContext.ActionDescriptor.ActionName}  Time : { timer.Elapsed.TotalSeconds}");
            }
        }

    }
}