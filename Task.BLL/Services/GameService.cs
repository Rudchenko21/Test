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
        private readonly ILoggingService _logger; // todo unused variable
        public GameService(IUnitOfWork db,ILoggingService _logger) // todo naming conventions, without underscore
        {
            this.db = db;
            this._logger = _logger;
        }
        //get all games // todo useless comment
        public ICollection<GameDTO> GetAll()
        {
            return Mapper.Map<ICollection<Game>, ICollection<GameDTO>>(this.db.Game.GetAll().ToList()); ; // todo extra ;. It's not very important, but you should strive to keep your code clean
        }
        public bool AddGame(GameDTO newGame)
        {
            if (newGame != null)
            {
                if (this.db.Game.Get(m => m.Name == newGame.Name).Count == 0) // todo please use Equals method with Ignoring case and !Any instead Count == 0. It will like: 
                                                                              // todo db.Game.Get(m => !m.Name.Equals(newGame.Name, StringComparison.InvariantCultureIgnoreCase)).Any()
                {
                    this.db.Game.Add(Mapper.Map<GameDTO, Game>(newGame));
                    db.SaveChanges();
                    return true;
                }
                else throw new ArgumentException("This object has already included in database"); // todo wrong exception
            }
            else throw new ArgumentNullException(); // todo add extra info here (for example what exact argument)
        }

        public bool ExistEntity(int Key) // todo wrong name of variable
        {
            return this.db.Game.Get(m => m.Id == Key).Count > 0;
        }

        public bool ExistStringKey(string Key)
        {
            return this.db.Game.Get(m => m.Key == Key).Count > 0;
        }
        public GameDTO GetGameByKey(int key) // todo rename this method. It's search by id, not by key
        {
            if (key > 0)
            {
                if (this.db.Game.Get(m => m.Id == key).Count == 1)
                {
                return Mapper.Map<Game, GameDTO>(this.db.Game.GetById(key));
               }
                throw new ArgumentNullException();
            }
            else throw new IndexOutOfRangeException(); // todo wrong and useless exception, just remove this verification
        }
        public GameDTO GetGameByNameKey(string key) // todo rename this method it's search by key, not by name key
        {
            if (key!=String.Empty) // todo please use method string.IsNullOrEmpty(key) instead of such verifications
            {
                    return Mapper.Map<Game, GameDTO>(this.db.Game.Get(m=>m.Key==key).ToList()[0]); // todo use Single here instead taking first element of array
            }
            else throw new ArgumentNullException("String is empty. Game's key can't be empty");
        }
        public IEnumerable<GameDTO> GetByGenre(int Key)
        {
            return Mapper.Map<IEnumerable<Game>,IEnumerable<GameDTO>>(this.db.Game.Get(m => m.Genres.All(m1 => m1.GenreId == Key))); // todo m1 - not good name for variable, add local variables here. It's hard to read, really.
        }
        public IEnumerable<GameDTO> GetAllByPlatformType(int Key)
        {
            return Mapper.Map<IEnumerable<Game>, IEnumerable<GameDTO>>(this.db.Game.Get(m => m.PlatformTypes.All(m1 => m1.Key == Key)));
        }
        public void DeleteGame(int key)
        {
            if (key >= 0) // todo no need this verification here
            {
                if (this.GetGameByKey(key)!=null)
                {
                    this.db.Game.Delete(key);
                    this.db.SaveChanges();
                }
                else throw new ArgumentNullException("No records were found with such key in database"); // todo wrong exception
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
