using System.Collections.Generic;

namespace Task.BLL.DTO
{
    public class CommentByGameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<CommentDTO> Comment { get; set; }
    }
}
