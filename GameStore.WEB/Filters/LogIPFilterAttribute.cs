using System.Web.Mvc;
using GameStore.WEB.Models;
using GameStore.BLL.Nlog;

namespace GameStore.WEB.Filters
{
    public class LogIPFilterAttribute: ActionFilterAttribute
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
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