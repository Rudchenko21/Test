using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllByPredicate(Expression<Func<T, bool>> filter, Expression<Func<T, bool>> name);
        T GetById(int id);
        void Add(T newItem);
        void Edit(T editItem);
        void Delete(int index);
    }
}
