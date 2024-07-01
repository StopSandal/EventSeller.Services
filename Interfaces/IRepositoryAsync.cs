using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for a generic repository that supports asynchronous operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepositoryAsync<TEntity>
    {
        /// <summary>
        /// Retrieves a collection of entities that match the specified filter, order, and includes.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <param name="orderBy">A function to order the entities.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include in the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        public Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
        public Task<TEntity> GetByIDAsync(object id);

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task InsertAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteAsync(object id);

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public void Delete(TEntity entityToDelete);
        /// <summary>
        /// Updates the specified entity in the repository.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        public void Update(TEntity entityToUpdate);
        /// <summary>
        /// Asynchronously determines whether any entities in the data source satisfy the specified condition.
        /// </summary>
        /// <param name="predicate">An expression to test each entity for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains 
        /// a boolean value that indicates whether any entity exists in the data source 
        /// that matches the specified condition.
        /// </returns>
        /// <example>
        /// <code>
        /// bool exists = await repository.DoesExistsAsync(entity => entity.Name == "ExampleName");
        /// </code>
        /// This example checks if there is any entity with the name "ExampleName" in the data source.
        /// </example>
        public Task<bool> DoesExistsAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Inserts a many new entities into the repository.
        /// </summary>
        /// <param name="entityList">The entity List to insert.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task InsertRangeAsync(IEnumerable<TEntity> entityList);
    }
}
