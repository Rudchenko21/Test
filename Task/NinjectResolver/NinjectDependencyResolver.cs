using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task.BLL.Interfaces;
using Task.BLL.Services;
using Task.BLL.NinjectDependencyResolver;

namespace Task.NinjectResolver
{
    public class NinjectDependencyResolver : IDependencyResolver

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
            kernel.Bind<IGameService>().To<GameService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            Task.BLL.NinjectDependencyResolver.NinjectResolver.Configure(kernel);
        }

    }
}