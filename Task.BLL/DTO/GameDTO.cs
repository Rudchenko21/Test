using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Entities;

namespace Task.BLL.DTO
{
    public class GameDTO: IEquatable<GameDTO>
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GenreDTO> Genre { get; set; }
        public IEnumerable<PlatformTypeDTO> PlatformType { get; set; }
        public bool Equals(GameDTO obj)
        {
            // Would still want to check for null etc. first.
            return this.Description == obj.Description &&
                this.Name == obj.Name;
        }

    }
}
