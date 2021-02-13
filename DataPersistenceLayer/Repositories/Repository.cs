using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
    /// <inheritdoc/>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The DbContext that the repository will work with.
        /// It is readonly because it can only be set on the Repository constructor.
        /// </summary>
        protected readonly DbContext _context;
        /// <summary>
        /// The <c>Repository</c> constructor.
        /// </summary>
        /// <param name="context">The DbContext that the repository will work with.</param>
        public Repository(DbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public TEntity Get(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            IEnumerable<TEntity> matchingResults = new List<TEntity>();
            if (predicate != null)
            {
                matchingResults = _context.Set<TEntity>().Where(predicate);
            }
            return matchingResults;
        }

        /// <inheritdoc/>
        public TEntity FindFirstOccurence(Func<TEntity, bool> predicate)
        {
            IEnumerable<TEntity> results = _context.Set<TEntity>().Where(predicate);
            if (results.Any())
            {
                return results.First();
            }
            return null;
        }

        /// <inheritdoc/>
        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                _context.Set<TEntity>().Add(entity);
            }
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                _context.Set<TEntity>().AddRange(entities);
            }
        }

        /// <inheritdoc/>
        public void Remove(TEntity entity)
        {
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }

        /// <inheritdoc/>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            if (entities != null && entities.Any())
            {
                _context.Set<TEntity>().RemoveRange(entities);
            }
        }
    }
}
