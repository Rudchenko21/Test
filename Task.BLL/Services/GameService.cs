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
        private readonly IUnitOfWork unitOfWork;
        public GameService(IUnitOfWork db)
        {
            this.unitOfWork = db;
        }
        public ICollection<GameDTO> GetAll()
        {
            return Mapper.Map<ICollection<Game>, ICollection<GameDTO>>(this.unitOfWork.Game.GetAll().ToList());
        }
        public bool AddGame(GameDTO newGame)
        {
            if (newGame != null)
            {
                if (!this.unitOfWork.Game.Get(m => m.Name.Equals(newGame.Name,StringComparison.InvariantCultureIgnoreCase)).Any()) 
                                                                              
                {
                    this.unitOfWork.Game.Add(Mapper.Map<GameDTO, Game>(newGame));
                    unitOfWork.SaveChanges();
                    return true;
                }
                else throw new ArgumentException("This object has already included in database"); // todo wrong exception
            }
            else throw new ArgumentNullException("Passed argument was null");
        }

        public bool ExistEntity(int Id) 
        {
            return this.unitOfWork.Game.Get(m => m.Id.Equals(Id)).Any();
        }

        public bool ExistEntity(string GameKey)
        {
            return this.unitOfWork.Game.Get(m => m.Key.Equals(GameKey,StringComparison.InvariantCultureIgnoreCase)).Any();
        }
        public GameDTO GetGameById(int Id)
        {
               if (this.unitOfWork.Game.Get(m => m.Id.Equals(Id)).Any())
                {
                    return Mapper.Map<Game, GameDTO>(this.unitOfWork.Game.GetById(Id));
                }
                else throw new ArgumentNullException();
        }
        public GameDTO SearchByKey(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                    return Mapper.Map<Game, GameDTO>(this.unitOfWork.Game.Get(m=>m.Key==key).Single());
            }
            else throw new ArgumentNullException("String is null or empty. Game's key can't be empty");
        }
        public IEnumerable<GameDTO> GetGamesByGenre(int Id)
        {
            return Mapper.Map<IEnumerable<Game>,IEnumerable<GameDTO>>(this.unitOfWork.Game.Get(m => m.Genres.All(genre => genre.Id== Id)));
        }
        public IEnumerable<GameDTO> GetGamesByPlatformType(int Id)
        {
            return Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(this.unitOfWork.Game.Get(m => m.PlatformTypes.All(platformType => platformType.Id == Id)));
        }
        public void DeleteGame(int key)
        {
                if (this.GetGameById(key)!=null)
                {
                    this.unitOfWork.Game.Delete(key);
                    this.unitOfWork.SaveChanges();
                }
                else throw new ArgumentNullException("No records were found with such key in database"); // todo wrong exception
        }
        public void EditGame(GameDTO item)
        {
            if (item != null)
            {
                this.unitOfWork.Game.Edit(Mapper.Map<GameDTO, Game>(item));
                unitOfWork.SaveChanges();
            }
            else throw  new ArgumentNullException("Model to edit is null");
        }
    }
}
