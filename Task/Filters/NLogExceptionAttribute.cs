using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Task.BLL.Models;
using Task.BLL.Nlog;

namespace Task.Filters
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