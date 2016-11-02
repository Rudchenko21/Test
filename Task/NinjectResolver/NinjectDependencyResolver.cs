using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task.BLL.Interfaces;
using Task.BLL.Services;
using Task.BLL.NinjectDependencyResolver;
using Task.BLL.Nlog;
using Ninject.Web.Mvc.FilterBindingSyntax;
using Task.Controllers;
using Task.Filters;

namespace Task.NinjectResolver
{
    public class NinjectDependencyResolver : IDependencyResolver // todo remove extra empty lines

    {

        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)

        {

            kernel = kernelParam;

            AddBindings();

        }

        public object GetService(Type serviceType)

        {

            return kernel.TryGet(serviceType);

        }

        public IEnumerable<object> GetServices(Type serviceType)

        {

            return kernel.GetAll(serviceType);

        }

        private void AddBindings()

        {
            kernel.Bind<IGameService>().To<GameService>(); // todo please move bindings to another class
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<ILoggingService>().To<LoggingService_>();
            kernel.BindFilter<LogIPFilterAttribute>(FilterScope.Controller, 0)
            .WhenControllerType<GameController>();

            Task.BLL.NinjectDependencyResolver.NinjectResolver.Configure(kernel);
        }

    }
}