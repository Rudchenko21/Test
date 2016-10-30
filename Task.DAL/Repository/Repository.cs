using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task.DAL.Context;
using Task.DAL.Interfaces;

namespace Task.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
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
        public virtual void Add(T newItem)
        {
            if (newItem != null)
            {
                this.entity.Attach(newItem);
                this.db.Entry(newItem).State = EntityState.Added;
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
        public IEnumerable<T> GetAllByInclude(Expression<Func<T, bool>> filter, string name)
        {
            return this.entity.Include(name).Where(filter);
        }
        public virtual ICollection<T> Get(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query =entity;
            if (filter != null)
            {
                query = query.Where(filter);
            }
                return query.ToList();
        }
        public virtual ICollection<T> GetAll()
        {
            if (this.Get() == null) return null;
            return this.Get();
        }
    }
}
