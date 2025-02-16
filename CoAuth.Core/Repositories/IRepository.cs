using System.Linq.Expressions;

namespace CoAuth.Core.Repositories;

public interface IRepository<TEntity> where TEntity:class
{
    Task<TEntity> GetByIdAsync(int id);

    IQueryable<TEntity> GetAll();

    IQueryable<TEntity> Where(Expression<Func<TEntity,bool>> predicate);

    Task AddAsync(TEntity entity);

    void Remove(TEntity entity);

    TEntity Update(TEntity entity);
}