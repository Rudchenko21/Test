using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Entities
{
    public class Comment
    {
        [Key]
        public int Key { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Game Game { get; set; }
    }
}
