using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class CommentByGameDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<CommentDTO> Comment { get; set; }
    }
}
