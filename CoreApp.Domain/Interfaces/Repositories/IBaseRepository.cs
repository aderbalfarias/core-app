﻿using System.Linq.Expressions;

namespace CoreApp.Domain.Interfaces.Repositories
{
    public interface IBaseRepository
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        Task<IEnumerable<TEntity>> GetAsync<TEntity>() where TEntity : class;
        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, string include) where TEntity : class;
        Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, string include) where TEntity : class;
        TEntity GetObject<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<TEntity> GetObjectAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<int> Add<TEntity>(TEntity entity);
        Task<int> Update<TEntity>(TEntity entity);
        Task<int> Delete<TEntity>(TEntity entity);
    }
}
