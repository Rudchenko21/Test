using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.DAL.Context;
using Task.BLL.Interfaces;
using Task.DAL.Interfaces;
using Task.DAL.UnitOfWork;
using AutoMapper;
using Task.BLL.Nlog;
using Task.DAL.Entities;

namespace Task.BLL.Services
{
    public class GameService:IGameService
    {
        private readonly IUnitOfWork db;
        private readonly ILoggingService _logger;
        public GameService(IUnitOfWork db,ILoggingService _logger)
        {
            this.db = db;
            this._logger = _logger;
        }
        //get all games
        public ICollection<GameDTO> GetAll()
        {
            return Mapper.Map<ICollection<Game>, ICollection<GameDTO>>(this.db.Game.GetAll().ToList()); ;
        }
        public bool AddGame(GameDTO newGame)
        {
            if (newGame != null)
            {
                if (this.db.Game.Get(m => m.Name == newGame.Name).Count == 0)
                {
                    this.db.Game.Add(Mapper.Map<GameDTO, Game>(newGame));
                    db.SaveChanges();
                    return true;
                }
                else throw new ArgumentException("This object has already included in database");
            }
            else throw new ArgumentNullException();
        }

        public bool ExistEntity(int Key)
        {
            return this.db.Game.Get(m => m.Id == Key).Count > 0;
        }

        public bool ExistStringKey(string Key)
        {
            return this.db.Game.Get(m => m.Key == Key).Count > 0;
        }
        public GameDTO GetGameByKey(int key)
        {
            if (key > 0)
            {
                if (this.db.Game.Get(m => m.Id == key).Count == 1)
                {
                return Mapper.Map<Game, GameDTO>(this.db.Game.GetById(key));
               }
                throw new ArgumentNullException();
            }
            else throw new IndexOutOfRangeException();
        }
        public GameDTO GetGameByNameKey(string key)
        {
            if (key!=String.Empty)
            {
                    return Mapper.Map<Game, GameDTO>(this.db.Game.Get(m=>m.Key==key).ToList()[0]);
            }
            else throw new ArgumentNullException("String is empty. Game's key can't be empty");
        }
        public IEnumerable<GameDTO> GetByGenre(int Key)
        {
            return Mapper.Map<IEnumerable<Game>,IEnumerable<GameDTO>>(this.db.Game.Get(m => m.Genres.All(m1 => m1.GenreId == Key)));
        }
        public IEnumerable<GameDTO> GetAllByPlatformType(int Key)
        {
            return Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(this.db.Game.Get(m => m.PlatformTypes.All(m1 => m1.Key == Key)));
        }
        public void DeleteGame(int key)
        {
            if (key >= 0)
            {
                if (this.GetGameByKey(key)!=null)
                {
                    this.db.Game.Delete(key);
                    this.db.SaveChanges();
                }
                else throw new ArgumentNullException("No records were found with such key in database");
            }
            else throw  new ArgumentException("Invalid key for delete");
        }
        public void Edit(GameDTO item)
        {
            if (item != null)
            {
                if (this.GetGameByKey(item.Id) != null)
                {
                    this.db.Game.Edit(Mapper.Map<GameDTO, Game>(item));
                    db.SaveChanges();
                }
            }
        }
    }
}
