using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Context;
using Task.DAL.Entities;
using Task.DAL.Interfaces;
using Task.DAL.Repository;

namespace Task.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameStoreContext _gameStoreContext;
        private Repository<Game> _gameRepository;
        private Repository<Comment> _commentRepository;
        private Repository<Genre> _genrereRepository;
        private Repository<PlatformType> _platformTypeRepository;

        public UnitOfWork(GameStoreContext gameStoreContext)
        {
            _gameStoreContext = gameStoreContext ?? new GameStoreContext();
        }
        public virtual IRepository<Game> Game
        {
            get
            {
                if (_gameRepository == null)
                    _gameRepository = new Repository<Game>(this._gameStoreContext);
                return this._gameRepository;
            }
        }

        public IRepository<Comment> Comment
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new Repository<Comment>(this._gameStoreContext);
                return this._commentRepository;
            }
        }
        public IRepository<Genre> Genre
        {
            get
            {
                if (_genrereRepository == null)
                    _genrereRepository = new Repository<Genre>(this._gameStoreContext);
                return this._genrereRepository;
            }
        }
        public IRepository<PlatformType> PlatformType
        {
            get
            {
                if (_platformTypeRepository == null)
                    _platformTypeRepository = new Repository<PlatformType>(this._gameStoreContext);
                return this._platformTypeRepository;
            }
        }
        public void SaveChanges()
        {
            this._gameStoreContext.SaveChanges();
        }
    }
}
