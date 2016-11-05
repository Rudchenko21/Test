using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; } 

        [StringLength(65)]
        [Index(IsUnique = true)]

        public  string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PlatformType> PlatformTypes { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
    }
}
