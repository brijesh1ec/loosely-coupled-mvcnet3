using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.Contracts
{
    /// <summary>
    /// Represents a domain wrapper of underlying Repository
    /// implementation classes should implement all the business logic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Int32 id);
        void Delete(T entity);
        void SaveChanges();
    }
}
