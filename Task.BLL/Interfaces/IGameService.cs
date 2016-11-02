using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.DAL.Entities;

namespace Task.BLL.Interfaces
{
    public interface IGameService
    {
        ICollection<GameDTO> GetAll();
        bool AddGame(GameDTO newGame);
        GameDTO GetGameByKey(int key);
        GameDTO GetGameByNameKey(string key);
        void DeleteGame(int key);
        void Edit(GameDTO item); // todo if you name all methods like DoSomethingWithEntity, plase use this convention everywhere
        IEnumerable<GameDTO> GetByGenre(int Key); // todo GetByGenre and GetAllByPlatformType, please use one convention
        IEnumerable<GameDTO> GetAllByPlatformType(int Key);
        bool ExistEntity(int Key); // todo i guess, id, not key
        bool ExistStringKey(string Key); // todo you can name it like ExistEntity too and variable will "gameKey"
    }

}
