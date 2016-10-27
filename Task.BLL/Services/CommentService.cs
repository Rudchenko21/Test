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
        public IEnumerable<CommentDTO> GetAll()
        {
            //return Mapper.Map<IEnumerable<Comment>,IEnumerable<CommentDTO>>(this.db.Comment.GetAllByPredicate(m=>m.Comments.Count==0,n=>n.Comments));
        }
    }
}
