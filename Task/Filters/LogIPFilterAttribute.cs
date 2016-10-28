using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task.Models;
using Task.BLL.Models;
using System.IO;
using Task.BLL.Nlog;

namespace Task.Filters
{
    public class LogIPFilterAttribute: ActionFilterAttribute,IActionFilter
    {
        
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILoggingService>();
            LogIPModel item = new LogIPModel
            {
                IP = filterContext.HttpContext.Request.UserHostAddress,
                Action= filterContext.ActionDescriptor.ActionName,
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
            };
            logger.Debug(item.ToString());
            this.OnActionExecuting(filterContext);
    }

}
}