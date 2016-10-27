using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.DAL.Entities;
using Task.DAL.Interfaces;

namespace Task.BLL.Services
{
    public class CommentService:ICommentService
    {
        private readonly IUnitOfWork db;

        public CommentService(IUnitOfWork db)
        {
            this.db = db;
        }
        public IEnumerable<CommentDTO> GetAllByGame(int Key)
        {
            return Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(this.db.Comment.GetAllByInclude(m=>m.Game.Key==Key,"Comments"));
        }
        public void AddCommentToGame(CommentDTO item)
        {
            if(item!=null)
            {
                this.db.Comment.Add(Mapper.Map<Comment>(item));this.db.SaveChanges();
            }
        }
    }
}
