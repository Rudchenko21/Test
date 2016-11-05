using System.Web.Routing;
using Moq;
using NUnit.Framework;
using GameStore.BLL.Interfaces;
using GameStore.Test.Stubs;
using GameStore.WEB;
using Assert = NUnit.Framework.Assert;

namespace GameStore.Test.Routes
{
    [TestFixture]
    public class RoutingMapTests
    {
        private readonly RouteCollection _routes;

        private readonly Mock<IGameService> _mockGameService;

        public RoutingMapTests()
        {
            _routes = RouteTable.Routes;
            RouteConfig.RegisterRoutes(_routes);
            _mockGameService=new Mock<IGameService>();
        }

        [TestCase("~/", "Game", "GetAllGames")]
        [TestCase("~/games", "Game", "GetAllGames")]
        [TestCase("~/game/GTA_5", "Game", "GetGameByKey")]
        [TestCase("~/games/1/download", "Game", "DownloadGameToFile")]
        [TestCase("~/games/new", "Game", "New")]
        [TestCase("~/games/update", "Game", "Update")]
        [TestCase("~/games/remove", "Game", "Remove")]
        [TestCase("~/game/GTA_5/comments", "Comment", "Comments")]
        [TestCase("~/game/GTA_5/newcomment", "Comment", "Newcomment")]
        public void DefaultRouteTest(string url, string expectedController,
            string expectedAction)
        {
            var context = new StubHttpContextForRouting(requestUrl: url);
            _mockGameService.Setup(m => m.ExistEntity(It.IsAny<string>())).Returns(false);
            // Act
            RouteData routeData = _routes.GetRouteData(context);
            // Assert
            Assert.AreEqual(expectedController.ToUpper(), ((string)routeData.Values["controller"]).ToUpper());
            Assert.AreEqual(expectedAction.ToUpper(), ((string)routeData.Values["action"]).ToUpper());
        }
    }
}
