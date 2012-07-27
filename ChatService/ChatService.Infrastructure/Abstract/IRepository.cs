using System;
using System.Linq;
using System.Linq.Expressions;

namespace ChatService.Infrastructure.Abstract
{
    public interface IRepository<TEntity> where TEntity:class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity tEntity);
        void Delete(TEntity tEntity);
    }
}
