using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task.ViewModel
{
    public class GenreViewModel
    {
        public int? ParentId { get; set; }
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}