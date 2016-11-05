using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.ViewModel
{
    public class GameViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<GenreViewModel> Genres { get; set; }

        public ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }

        public override string ToString()
        {
            return $"Game key : {Key} \n Game Id : {Id}";
        }
    }
}