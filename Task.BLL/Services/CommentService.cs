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
        private readonly IUnitOfWork db; // todo please use _underscoreConvention, so than you'll not need use this.db
        // todo rename db to unitOfWork, or something like this. It's not database, it's UnitOfWork.

        public CommentService(IUnitOfWork db)
        {
            this.db = db;
        }
        public ICollection<CommentDTO> GetAllByGame(string Key)
        {
                return
                    Mapper.Map<ICollection<Comment>, ICollection<CommentDTO>>(
                        this.db.Comment.Get(m => m.Game.Key == Key).ToList()); // todo use additional variables
        }

        public bool ExistEntity(string Key)
        {
            return this.db.Comment.Get(m => m.Game.Key== Key).Count > 0; // todo you can use .Any() instead .Count > 0
        }
        public bool AddCommentToGame(CommentDTO item)
        {
            if(item!=null) // todo add spaces, please. It's pretty hard to read
            {
                if (!ExistEntity(item.GameKey))
                {
                    this.db.Comment.Add(Mapper.Map<Comment>(item));// todo use additional variables
                    this.db.SaveChanges();
                    return true; // todo make sense introduse this to variable and make one return statement
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
