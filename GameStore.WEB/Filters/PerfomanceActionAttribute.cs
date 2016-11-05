using System.Diagnostics;
using System.Web.Mvc;
using GameStore.BLL.Nlog;

namespace GameStore.WEB.Filters
{
    public class PerfomanceActionAttribute: System.Web.Mvc.FilterAttribute, IActionFilter
    {
        private Stopwatch _timer;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _timer = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILoggingService>();
            _timer.Stop();
            if (filterContext.Exception == null)
            {
                //logger.Info($"Controller: {filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}  ActionName : {filterContext.ActionDescriptor.ActionName}  Time : { _timer.Elapsed.TotalSeconds}");
            }
        }

    }
}