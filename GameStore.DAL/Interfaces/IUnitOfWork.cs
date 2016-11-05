using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Comment> Comment { get; }
        IRepository<Game> Game { get; }
        IRepository<Genre> Genre { get; }
        IRepository<PlatformType> PlatformType { get; }

        void SaveChanges();
    }
}
