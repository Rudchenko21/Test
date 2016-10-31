using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.DAL.Context;
using Task.BLL.Interfaces;
using Task.DAL.Interfaces;
using Task.DAL.UnitOfWork;
using AutoMapper;
using Task.DAL.Entities;

namespace Task.BLL.Services
{
    public class GameService:IGameService
    {
        private readonly IUnitOfWork db;
        public GameService(IUnitOfWork db)
        {
            this.db = db;
        }
        //get all games
        public ICollection<GameDTO> GetAll()
        {
            if(this.db.Game.GetAll().ToList().Count>0)
            {
                return Mapper.Map<ICollection<Game>, ICollection<GameDTO>>(this.db.Game.GetAll().ToList());
            }
            return null;
        }
        public void AddGame(GameDTO newGame)
        {
            if (newGame != null)
            {
                if (this.db.Game.Get(m => m.Name == newGame.Name).Count == 0)
                {
                    this.db.Game.Add(Mapper.Map<GameDTO, Game>(newGame));
                    db.SaveChanges();
                }
            } 
        }
        public bool ExistEntity(int Key)
        {
            return this.db.Game.Get(m => m.Key == Key).Count > 0;
        }
        public GameDTO GetGameByKey(int key)
        {
            if (key > 0)
            {
                if (this.db.Game.Get(m => m.Key == key).Count == 0) { 
                return Mapper.Map<Game, GameDTO>(this.db.Game.GetById(key));
               }return null;
            }
            else return null;
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
            if (key != 0)
            {
                if (this.GetGameByKey(key)!=null)
                {
                    this.db.Game.Delete(key);
                    this.db.SaveChanges();
                }
            }
        }
        public void Edit(GameDTO item)
        {
            if (item != null)
            {
                if (this.GetGameByKey(item.Key) != null)
                {
                    this.db.Game.Edit(Mapper.Map<GameDTO, Game>(item));
                    db.SaveChanges();
                }
            }
        }
    }
}
