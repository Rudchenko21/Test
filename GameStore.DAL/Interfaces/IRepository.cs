﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        ICollection<T> Get(Expression<Func<T, bool>> filter = null);
        ICollection<T> GetAll();
        T GetById(int id);
        void Add(T newItem);
        void Edit(T editItem);
        void Delete(int index);
    }
}
