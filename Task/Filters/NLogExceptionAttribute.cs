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
                ExceptionClass exitem = new ExceptionClass
                {
                    Message = filterContext.Exception.Message,
                    StackTrace = filterContext.Exception.StackTrace,
                    TargetSite = filterContext.Exception.TargetSite.ToString(),
                    StatusCode= filterContext.HttpContext.Response.StatusCode
                };
                
                logger.Error(exitem.ToString()); // todo maybe make sense log just filterContext.Exception here without extra exception..? 
            }
            filterContext.ExceptionHandled = true;
        }
    }
    }