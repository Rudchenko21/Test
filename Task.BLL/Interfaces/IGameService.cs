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
        IEnumerable<GameDTO> GetAll();
        void AddGame(GameDTO newGame);
        GameDTO GetGameByKey(int key);
        void DeleteGame(int key);
        void Edit(GameDTO item);
    }

}
