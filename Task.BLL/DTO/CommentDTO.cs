using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.DTO
{
    public class CommentDTO
    {
        public int Key { get; set; }
        public int GameKey { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
