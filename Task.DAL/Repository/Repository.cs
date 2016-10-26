using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Context;
using Task.DAL.Interfaces;

namespace Task.DAL.Repository
{
    class Repository<T> : IRepository<T> where T : class
    {
        private readonly GameStoreContext db;
        private DbSet<T> entity;

        public Repository(GameStoreContext db)
        {
            if (db != null)
            {
                this.db = db;
                entity = db.Set<T>();
            }
        }
        public T GetById(int Id)
        {
            return this.entity.Find(Id);
        }
        public void Add(T newItem)
        {
            if (newItem != null)
            {
                this.entity.Add(newItem);
            }
        }
        public void Delete(int Id)
        {
            T item = this.entity.Find(Id);
            this.entity.Remove(item);
        }
        public void Edit(T editItem)
        {
            if (editItem != null)
            {
                this.db.Entry(editItem).State = EntityState.Modified;
            }
        }
        public IEnumerable<T> GetAll()
        {
            return this.entity;
        }
    }
}
