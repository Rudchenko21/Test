using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WEB.Models
{
    public class LogIPModel
    {
        public string IP { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public override string ToString()
        {
            return $"Ip : {IP}  Action : {Action}  Controller : {Controller}";
        }
    }
}