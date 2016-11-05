using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Interfaces;
using AutoMapper;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Services
{
    public class GameService:IGameService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameService(IUnitOfWork db)
        {
            this._unitOfWork = db;
        }
        public ICollection<GameDTO> GetAll()
        {
            return Mapper.Map<ICollection<Game>, ICollection<GameDTO>>(_unitOfWork.Game.GetAll().ToList());
        }
        public bool AddGame(GameDTO newGame)
        {
            if (newGame != null)
            {
                if (!_unitOfWork.Game.Get(m => m.Name.Equals(newGame.Name,StringComparison.InvariantCultureIgnoreCase)).Any()) 
                                                                              
                {
                    _unitOfWork.Game.Add(Mapper.Map<GameDTO, Game>(newGame));
                    _unitOfWork.SaveChanges();
                    return true;
                }
                else throw new InvalidOperationException("This object has already included in database");
            }
            else throw new ArgumentNullException("Passed argument was null");
        }

        public bool ExistEntity(int Id) 
        {
            return _unitOfWork.Game.Get(m => m.Id.Equals(Id)).Any();
        }

        public bool ExistEntity(string GameKey)
        {
            return _unitOfWork.Game.Get(m => m.Key.Equals(GameKey,StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public GameDTO GetGameById(int Id)
        {
               if (_unitOfWork.Game.Get(m => m.Id.Equals(Id)).Any())
                {
                    return Mapper.Map<Game, GameDTO>(_unitOfWork.Game.GetById(Id));
                }
                else throw new ArgumentNullException();
        }

        public GameDTO SearchByKey(string key)
        {
            if (!String.IsNullOrEmpty(key))
            {
                    return Mapper.Map<Game, GameDTO>(_unitOfWork.Game.Get(m=>m.Key==key).Single());
            }
            else throw new ArgumentNullException("String is null or empty. Game's key can't be empty");
        }

        public IEnumerable<GameDTO> GetGamesByGenre(int Id)
        {
            return Mapper.Map<IEnumerable<Game>,IEnumerable<GameDTO>>(_unitOfWork.Game.Get(m => m.Genres.All(genre => genre.Id== Id)));
        }

        public IEnumerable<GameDTO> GetGamesByPlatformType(int Id)
        {
            return Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(_unitOfWork.Game.Get(m => m.PlatformTypes.All(platformType => platformType.Id == Id)));
        }

        public void DeleteGame(int id)
        {
                if (GetGameById(id)!=null)
                {
                    _unitOfWork.Game.Delete(id);
                    _unitOfWork.SaveChanges();
                }
                else throw new ArgumentNullException("No records were found with such key in database"); 
        }

        public void EditGame(GameDTO item)
        {
            if (item != null)
            {
                _unitOfWork.Game.Edit(Mapper.Map<GameDTO, Game>(item));
                _unitOfWork.SaveChanges();
            }
            else throw  new ArgumentNullException("Model to edit is null");
        }
    }
}
