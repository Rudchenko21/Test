using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.WEB.NinjectResolver
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            NinjectResolver.Configure(_kernel);
            GameStore.BLL.NinjectDependencyResolver.NinjectResolver.Configure(_kernel);
        }

    }
}