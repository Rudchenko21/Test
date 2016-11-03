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
        GameDTO GetGameById(int Id);
        GameDTO SearchByKey(string key);
        void DeleteGame(int Id);
        void EditGame(GameDTO item);
        IEnumerable<GameDTO> GetGamesByGenre(int Id);
        IEnumerable<GameDTO> GetGamesByPlatformType(int Key);
        bool ExistEntity(int Id); 
        bool ExistEntity(string gamekey);
    }

}
