namespace CoreApp.Domain.Interfaces.Repositories
{
    public interface IBaseRepository
    {
        IQueryable<TEntity> Get<TEntity>();
        Task<IEnumerable<TEntity>> GetAsync<TEntity>();

        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, string include) where TEntity : class;
        Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate);

        TEntity GetObject<TEntity>(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetObjectAsync<TEntity>(Expression<Func<TEntity, bool>> predicate);

        Task<int> Add<TEntity>(TEntity entity);
        Task<int> Update<TEntity>(TEntity entity);
    }
}
