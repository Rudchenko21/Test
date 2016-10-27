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
        public IEnumerable<GameDTO> GetAll()
        {
            return Mapper.Map<IEnumerable<Game>,IEnumerable<GameDTO>>(this.db.Game.GetAll().ToList());
        }
        public void AddGame(GameDTO newGame)
        {
            if (newGame != null)
            {
                if (!Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(this.db.Game.GetAll().ToList()).Contains(newGame))
                {
                    this.db.Game.Add(Mapper.Map<GameDTO, Game>(newGame));
                    db.SaveChanges();
                }
            }
            
        }
        public GameDTO GetGameByKey(int key)
        {
            if (key > 0)
            {
                if (this.db.Game.GetAll().Where(m => m.Key == key).Count() != 0)
                {
                    return Mapper.Map<Game, GameDTO>(this.db.Game.GetById(key));
                }
                else return null;
            }
            else return null;
        }
        public void DeleteGame(int key)
        {
            if (key != 0)
            {
                if (this.db.Game.GetAll().Where(m => m.Key == key).Count() > 0)
                {
                    this.db.Game.Delete(key);
                    this.db.SaveChanges();
                }

            }
        }
        //public IEnumerable<GameDTO> GetGamesByPlatform(int Id)
        //{

        //}
        public void Edit(GameDTO item)
        {
            if (item != null)
            {
                    this.db.Game.Edit(Mapper.Map<GameDTO, Game>(item));
                    db.SaveChanges();
            }
        }
    }
}
