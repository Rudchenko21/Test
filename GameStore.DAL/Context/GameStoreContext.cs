using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Context
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext() : base("GameStore")
        {
        }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<PlatformType> PlatformTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasMany(x => x.Comments).WithRequired(y => y.Game).WillCascadeOnDelete(true);
        }

    }
}
