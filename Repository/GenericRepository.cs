using EventSeller.DataLayer.EF;
using EventSeller.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSeller.Services.Repository
{
    /// <summary>
    /// A generic implementation of <see cref="IRepositoryAsync{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <inheritdoc cref="IRepositoryAsync{TEntity}"/>
    public class GenericRepository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        internal SellerContext context;
        internal DbSet<TEntity> dbSet;
        /// <summary>
        /// An initializing new instance of the  <see cref="GenericRepository{TEntity}"/> class with specified database context
        /// </summary>
        /// <param name="context">The database context</param>
        public GenericRepository(SellerContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
        /// <inheritdoc/>
        public async virtual Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        /// <inheritdoc/>
        public async virtual Task<bool> DoesExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }
        /// <inheritdoc/>
        public async virtual Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        /// <inheritdoc/>
        public async virtual Task InsertRangeAsync(IEnumerable<TEntity> entityList)
        {
            await dbSet.AddRangeAsync(entityList);
        }
        /// <inheritdoc/>
        public async virtual Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await GetByIDAsync(id);
            Delete(entityToDelete);
        }
        /// <inheritdoc/>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        /// <inheritdoc/>
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TField>> GetFieldValuesAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> selector)
        {
            return await context.Set<TEntity>()
                .Where(filter)
                .Select(selector)
                .ToListAsync();
        }
        /// <inheritdoc/>
        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null, IEnumerable<string> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.CountAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<decimal> GetAverageAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> filter = null, IEnumerable<string> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AverageAsync(selector);
        }
        /// <inheritdoc/>
        public virtual async Task<decimal> GetSumAsync(Expression<Func<TEntity, decimal>> selector, Expression<Func<TEntity, bool>> filter = null, IEnumerable<string> includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.SumAsync(selector);
        }
    }
}
