using Ninject;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Nlog;
using GameStore.BLL.Services;

namespace GameStore.WEB.NinjectResolver
{
    public class NinjectResolver
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<IGameService>().To<GameService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<ILoggingService>().To<LoggingService_>();
        }
    }
}