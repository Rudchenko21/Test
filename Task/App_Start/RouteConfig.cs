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
                name: "gameGetAllGames",
                url: "games/",
                defaults: new { controller = "Game", action = "GetAllGames" }
            );
            routes.MapRoute(
                "UpdateGame",
                url: "games/update",
                defaults: new { controller = "Game", action = "UpdateGame" }
            );
            routes.MapRoute(
                "RemoveGame",
                url: "games/remove",
                defaults: new { controller = "Game", action = "RemoveGame" }
            );
            routes.MapRoute(
                "CreateGame",
                url: "games/new",
                defaults: new { controller = "Game", action = "AddGame" }
            );
            routes.MapRoute(
               name: "GameInfo",
               url: "games/{key}",
               defaults: new { controller = "Game", action = "GetGameByKey" }
           );
            routes.MapRoute(
                name: "GetCommentByName",
                url: "games/{gamekey}/comment",
                defaults: new { controller = "Game", action = "GetAllCommentsByGames" },
                constraints: new { gameKey = @"^[A-Za-z0-9_-]{2,20}", action = @"\w+" }
            );
            routes.MapRoute(
                name: "CreateNewComment",
                url: "games/{gamekey}/newcomment",
                defaults: new { controller = "Game", action = "AddCommentToGame" },
               constraints: new { gameKey = @"^[A-Za-z0-9_-]{2,20}", action = @"\w+" }
            );
            routes.MapRoute(
                name: "DownloadGame",
                url: "games/{gamekey}/download",
                defaults: new { controller = "Game", action = "DownloadGameToFile" }
            );
            
            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Game", action = "GetAllGames", id=UrlParameter.Optional }
             );
        }
    }
}
