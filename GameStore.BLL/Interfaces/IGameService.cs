using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
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
