using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Entities;

namespace Task.DAL.Context
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext() : base("GameStore")
        {
        }
        public virtual DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // настройка полей с помощью Fluent API
            modelBuilder.Entity<Comment>().HasKey(m => m.Key);
            modelBuilder.Entity<Game>().HasKey(m => m.Key);
            modelBuilder.Entity<Genre>().HasKey(m => m.GenreId);
            modelBuilder.Entity<PlatformType>().HasKey(m => m.Key);
            base.OnModelCreating(modelBuilder);
        }

    }
}
