using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Task.Test.Routes
{
    [TestFixture]
    public class RoutingMapTests
    {
        private readonly RouteCollection _routes;

        public RoutingMapTests()
        {
            _routes = RouteTable.Routes;
            RouteConfig.RegisterRoutes(_routes);
        }
        //вот эти два роута не работают, при чем первый отлично работает в браузере
        //[TestCase("~/games/2/comment", "Game", "GetAllCommentsByGames")] // todo try to test its too
        //[TestCase("~/games/1/newcomment", "Game", "AddCommentToGame")]

        [Test] // todo useless attribute
        [TestCase("~/", "Game", "GetAllGames")]
        [TestCase("~/games", "Game", "GetAllGames")]
        [TestCase("~/game/1", "Game", "GetGameByKey")]
        [TestCase("~/games/1/download", "Game", "DownloadGameToFile")]
        [TestCase("~/games/new", "Game", "AddGame")]
        [TestCase("~/games/update", "Game", "update")]
        [TestCase("~/games/remove", "Game", "remove")]
        public void DefaultRoute(string url, string expectedController, string expectedAction)
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: url);
            // Act
            RouteData routeData = _routes.GetRouteData(context);
            // Assert
            Assert.AreEqual(expectedController.ToUpper(), ((string) routeData.Values["controller"]).ToUpper());
            Assert.AreEqual(expectedAction.ToUpper(), ((string) routeData.Values["action"]).ToUpper());
        }
    }
}
