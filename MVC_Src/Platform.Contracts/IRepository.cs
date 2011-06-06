using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platform.Contracts
{

    public interface IRepository<T>
    {

        /// <summary>
        /// Retrieves all the entities.
        /// </summary>
        /// <returns>An <see cref="IQueryable"/> containing all the entities.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get entity by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Int32 id);

        /// <summary>
        /// The common DB context;
        /// </summary>
        IDBContext DBContext { get; set; }

        /// <summary>
        /// Adds the entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Deletes the entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void Delete(T entity);

        /// <summary>
        /// Saves the changes to the repository.
        /// </summary>
        void SaveChanges();
    }
}
