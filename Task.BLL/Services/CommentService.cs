using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.BLL.Nlog;
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
        public ICollection<CommentDTO> GetAllByGame(string Key)
        {
                return
                    Mapper.Map<ICollection<Comment>, ICollection<CommentDTO>>(
                        this.db.Comment.Get(m => m.Game.Key == Key).ToList());
        }

        public bool ExistEntity(string Key)
        {
            return this.db.Comment.Get(m => m.Game.Key== Key).Count > 0;
        }
        public bool AddCommentToGame(CommentDTO item)
        {
            if(item!=null)
            {
                if (!ExistEntity(item.GameKey))
                {
                    this.db.Comment.Add(Mapper.Map<Comment>(item));
                    this.db.SaveChanges();
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }
    }
}
