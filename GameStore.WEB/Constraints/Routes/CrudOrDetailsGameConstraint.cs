using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Nlog;

namespace GameStore.WEB.Constraints.Routes
{
    public class CrudOrDetailsGameConstraint:IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            bool result = true;  
            switch (values[parameterName].ToString())
            {
                case "game":
                    return SelectPathWithGame(values);
                case "games":
                    return SelectPathWithGames(values);
                default:
                    return false;
            }
        }

        private bool SelectPathWithGames(RouteValueDictionary values)
        {
            string[] actionNames = new string[] { "new", "update", "remove"};
            if (actionNames.Contains(values["key"].ToString().ToLower()))
            {
                values["action"] = values["key"].ToString();
                return true;
            }
            else
            {
                values["action"] = "GetAllGames";
                return true;
            }
            return false;
        }

        private bool SelectPathWithGame(RouteValueDictionary values)
        {
            bool routeSwitch = String.IsNullOrEmpty(values["key"].ToString());
            values["action"] = !routeSwitch ? "GetGameByKey" : values["key"].ToString();
            return true;
        }

    }
}