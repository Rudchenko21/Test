using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; // todo remove all unused namesaces. How to do it for project or solution can be found here:
                  // todo https://www.jetbrains.com/help/resharper/2016.2/Refactorings__Remove_Unused_References.html
using System.Web.Mvc;
using System.Web.Routing;

namespace Task
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) 
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // todo try to optimize routes.
            // todo for example, next 4 routes can be written via one configuration, not 4.

            routes.MapRoute(
                name: "gameGetAllGames",
                url: "games/",
                defaults: new {controller = "Game", action = "GetAllGames"}
            );
            routes.MapRoute(
               name: "GameInfo",
               url: "game/{key}",
               defaults: new { controller = "Game", action = "GetGameByKey" }
           );
            routes.MapRoute(
                "CreateGame",
                url: "games/new",
                defaults: new { controller = "Game", action = "AddGame" }
            );
            routes.MapRoute(
                "UpdateGame",
                url: "games/{action}",
                defaults: new { controller = "Game", action = "update" }
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
