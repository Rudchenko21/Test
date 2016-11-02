using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Entities
{
    public class Genre
    {
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        [Key]
        public int GenreId { get; set; }
        [StringLength(65)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public virtual ICollection<Game> Games { get; set; }

        public virtual Genre Parent { get; set; }
    }
}
