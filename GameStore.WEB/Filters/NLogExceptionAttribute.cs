using System.Web.Mvc;
using GameStore.BLL.Nlog;

namespace GameStore.WEB.Filters
{
    public class NLogExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var logger = DependencyResolver.Current.GetService<ILoggingService>();
            if (!filterContext.ExceptionHandled)
            {
                logger.Error(filterContext.Exception);
            }
            filterContext.ExceptionHandled = true;
        }
    }
    }