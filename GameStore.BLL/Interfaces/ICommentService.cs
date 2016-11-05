using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface ICommentService
    {
        ICollection<CommentDTO> GetAllByGame(string Key);

        void AddCommentToGame(CommentDTO item);
    }
}
