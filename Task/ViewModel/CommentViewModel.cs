using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task.ViewModel
{
    public class CommentViewModel
    {
        public int Key { get; set; }
        public int GameKey { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}