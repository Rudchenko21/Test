using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Entities
{
    public class PlatformType
    {
        [Key]
        public int Key { get; set; }
        [StringLength(65)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
