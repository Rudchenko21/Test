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
        void Edit(GameDTO item);
        IEnumerable<GameDTO> GetByGenre(int Key);
        IEnumerable<GameDTO> GetAllByPlatformType(int Key);
        bool ExistEntity(int Key);
        bool ExistStringKey(string Key);
    }

}
