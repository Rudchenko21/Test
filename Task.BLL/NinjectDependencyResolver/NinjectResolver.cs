using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Interfaces;
using Task.DAL.UnitOfWork;

namespace Task.BLL.NinjectDependencyResolver
{
    public class NinjectResolver
    {

        public static void Configure(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
