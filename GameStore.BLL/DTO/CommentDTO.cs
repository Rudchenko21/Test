using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string GameKey { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
