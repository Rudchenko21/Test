using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.BLL.DTO
{
    public class CommentByGameDTO
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<CommentDTO> Comment { get; set; }
    }
}
