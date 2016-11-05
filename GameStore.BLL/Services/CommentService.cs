using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Services
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
            if (!String.IsNullOrEmpty(key))
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

        public void AddCommentToGame(CommentDTO item)
        {
            if(item != null) 
            {
                if ( !ExistEntity (item.GameKey) )
                {
                    var comment = Mapper.Map<Comment>(item);
                    this._unitOfWork.Comment.Add(comment);
                    this._unitOfWork.SaveChanges();
                }
                else throw  new InvalidOperationException("Try to add existence entity");
            }
            else
            {
                throw new ArgumentNullException("Try to add null object");
            }
        }
    }
}
