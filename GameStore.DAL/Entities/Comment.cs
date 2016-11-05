using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; } 

        public int GameId { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public virtual Game Game { get; set; }
    }
}
