﻿using DataLayer.Model.EF;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using EventSeller.Services.Interfaces;

namespace Services.Repository
{
    /// <summary>
    /// A generic implementation of <see cref="IRepositoryAsync{TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <inheritdoc cref="IRepositoryAsync{TEntity}"/>
    public class GenericRepository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class { 
        internal SellerContext context;
        internal DbSet<TEntity> dbSet;
        /// <summary>
        /// An initializing new instance of the  <see cref="GenericRepository{TEntity}"/> class with specified database context
        /// </summary>
        /// <param name="context">The database context</param>
        public GenericRepository(SellerContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> Get(
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
        public async virtual Task<TEntity> GetByID(object id)
        {
            return await dbSet.FindAsync(id);
        }
        /// <inheritdoc/>
        public async virtual Task Insert(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        /// <inheritdoc/>
        public async virtual Task Delete(object id)
        {
            TEntity entityToDelete = await GetByID(id);
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
    }
}
