using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Task.BLL.DTO;
using Task.BLL.Interfaces;
using Task.DAL.Entities;
using Task.DAL.Interfaces;

namespace Task.BLL.Services
{
    public class CommentService:ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public ICollection<CommentDTO> GetAllByGame(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                var list = Mapper.Map<ICollection<Comment>, ICollection<CommentDTO>>(this._unitOfWork.Comment.Get(game => game.Game.Key == key).ToList());
                return list;
            }
            else throw new ArgumentNullException("Try to get comments by gamekey with null or empty key ");
        }

        public bool ExistEntity(string key)
        {
            return this._unitOfWork.Comment.Get(m => m.Game.Key.Equals(key,StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public bool AddCommentToGame(CommentDTO item)
        {
            bool state = false;
            if(item != null) 
            {
                if ( !ExistEntity (item.GameKey) )
                {
                    var comment = Mapper.Map<Comment>(item);
                    this._unitOfWork.Comment.Add(comment);
                    this._unitOfWork.SaveChanges();
                    state = true; 
                    return state;
                }
                else return state;
            }
            else
            {
                return state;
            }
        }
    }
}
