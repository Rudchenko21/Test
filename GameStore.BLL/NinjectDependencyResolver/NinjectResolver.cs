using Ninject;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Models;
using GameStore.DAL.Interfaces;
using GameStore.DAL.UnitOfWork;

namespace GameStore.BLL.NinjectDependencyResolver
{
    public class NinjectResolver
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IWriter>().To<TxtWriter>();
        }
    }
}
