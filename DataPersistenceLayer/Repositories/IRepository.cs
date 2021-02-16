using System;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    /// <summary>
    /// The <c>IRepository</c> interface.
    /// Defines the basic operations for working with the entities from
    /// the persistence layer.
    /// This basic operations are:
    /// <list type="bullet">
    /// <item>
    /// <term>Get</term>
    /// <description>Retrieves the entity that possess the specified primary key.</description>
    /// </item>
    /// <item>
    /// <term>GetAll</term>
    /// <description>Retrieves all the entities from the database.</description>
    /// </item>
    /// <item>
    /// <term>Find</term>
    /// <description>Retrieves all the entities that meet the given condition.</description>
    /// </item>
    /// <item>
    /// <term>FindFirsOccurence</term>
    /// <description>Retrieves the first entity that meets the given condition.</description>
    /// </item>
    /// <item>
    /// <term>Add</term>
    /// <description>Adds the given entity.</description>
    /// </item>
    /// <item>
    /// <term>AddRange</term>
    /// <description>Adds the given entities.</description>
    /// </item>
    /// <item>
    /// <term>Remove</term>
    /// <description>Removes the given entity.</description>
    /// </item>
    /// <item>
    /// <term>RemoveRange</term>
    /// <description>Removes the given entities.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <typeparam name="TEntity">The entity the repository will work with.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves the entity that possess the specified primary key.
        /// </summary>
        /// <param name="id">the primary key of the entity you would like to look for on the database</param>
        /// <returns>The entity that possess the specified key, or null is the entity does not exist.</returns>
        TEntity Get(object id);

        /// <summary>
        /// Retrieves all the entities from the database.
        /// </summary>
        /// <returns>All the entities from the database, or an empty collection if there are no entities.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Retrieves all the entities that meet the given condition.
        /// </summary>
        /// <param name="predicate">The condition that entities need to meet to be included in the returned collection</param>
        /// <returns>All the entities that meet the given condition, or an empty list if no entity met the condition</returns>
        IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);

        /// <summary>
        /// Retrieves the first entity that meets the given condition.
        /// </summary>
        /// <param name="predicate">The condition that an entity need to meet to be returned</param>
        /// <returns>The first entity that meets the given condition, or null if no entity met the condition</returns>
        TEntity FindFirstOccurence(Func<TEntity, bool> predicate);

        /// <summary>
        /// Adds the given entity.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Adds the given entities.
        /// </summary>
        /// <param name="entities">The collection of entities to be added</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes the given entity.
        /// Note that you need to pass exactly the same entity that you want to remove.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes the given collection of entities.
        /// </summary>
        /// <param name="entities">the collection of entities to be removed.</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
