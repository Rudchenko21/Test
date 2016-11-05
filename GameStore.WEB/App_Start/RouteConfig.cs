using System.Web.Mvc;
using System.Web.Routing;
using GameStore.WEB.Constraints.Routes;

namespace GameStore.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) 
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "",
                url: "{param}/{key}",
                defaults: new { controller = "Game", key = UrlParameter.Optional },
                constraints: new { param = new CrudOrDetailsGameConstraint() }
            );
            routes.MapRoute(
                name:"",
                url:"game/{gamekey}/{action}",
                defaults: new { controller = "Comment", action = "Comments" },
                constraints: new { gameKey = @"^[A-Za-z0-9_-]{2,20}", action = @"\w+" }
            );
            routes.MapRoute(
                name: "DownloadGame",
                url: "games/{gamekey}/download",
                defaults: new { controller = "Game", action = "DownloadGameToFile" }
            );
            routes.MapRoute(
                name:"Default",
                url:"{controller}/{action}/{id}",
                defaults: new {controller="Game",action="GetAllGames",id=UrlParameter.Optional}
            );
        }
    }
}
