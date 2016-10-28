using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task.ViewModel
{
    public class GameViewModel
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<GenreViewModel> Genres { get; set; }
        public ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }
        public override string ToString()
        {
            return $"Game name : {Name} \n Game description : {Description} \n Game genres: {Genres.ToList().Select(m=>m.Name+"\n")} \n Game platformtype : {PlatformTypes.ToList().Select(m=>m.Name+"\n")}";
        }
    }
}