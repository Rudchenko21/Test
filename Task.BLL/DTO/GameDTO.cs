﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Entities;

namespace Task.BLL.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public override bool Equals(object obj)
        {
            var a = obj as GameDTO;
            return this.Description == a.Description &&
                this.Name == a.Name && this.Key==a.Key;;
        }
    }
}
