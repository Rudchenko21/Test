using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task.ViewModel
{
    public class GameViewModel
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GenreViewModel> Genre { get; set; }
        public IEnumerable<PlatformTypeViewModel> PlatformType { get; set; }
    }
}