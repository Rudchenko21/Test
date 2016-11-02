using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.DAL.Entities;

namespace Task.BLL.Interfaces
{
    public interface ICommentService
    {
        ICollection<CommentDTO> GetAllByGame(string Key);
        bool AddCommentToGame(CommentDTO item);
    }
}
