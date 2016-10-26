using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.DAL.Repository
{
    internal class IDBSet<T> where T : class
    {
        internal void Find<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
