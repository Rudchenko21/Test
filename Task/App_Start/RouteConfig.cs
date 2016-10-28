using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Task
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "gameDetails",
                url: "games/{key}",
                defaults: new { controller = "Game", action = "GetGameByKey" },
                constraints: new {key= @"\d +"}
            );
            routes.MapRoute(
                null,
                url: "games/{action}",
                defaults: new { controller = "Game", action = "update" }
            );
            routes.MapRoute(
                null,
                url: "games/new",
                defaults: new { controller = "Game", action = "AddGame" }
            );

            routes.MapRoute(
                name: "add1",
                url: "games/{gamekey}/newcomment",
                defaults: new { controller = "Game", action = "AddCommentToGame" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );
            routes.MapRoute(
                name: "download_game",
                url: "game/{gamekey}/download",
                defaults: new { controller = "Game", action = "DownloadGameToFile" }
            );
            
            routes.MapRoute(
                name: "get_comments",
                url: "game/{gamekey}/comment",
                defaults: new { controller = "Game", action = "GetAllCommentsByGames" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "GetAllGames", id = UrlParameter.Optional }
            );
        }
    }
}
