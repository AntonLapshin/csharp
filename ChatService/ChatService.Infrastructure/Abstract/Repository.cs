using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ChatService.Infrastructure.Abstract
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private readonly IContextProvider _contextProvider;

        protected Repository(IContextProvider contextProvider)
        {
            if (contextProvider == null)
                throw new ArgumentNullException("contextProvider");
            _contextProvider = contextProvider;
        }


        protected DbContext Context
        {
            get { return _contextProvider.CurrentContext; }
        }

        protected DbSet<TEntity> DbSet
        {
            get
            {
                return Context.Set<TEntity>();
            }
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Add(TEntity tEntity)
        {
            DbSet.Add(tEntity);
            Save(); //ugly UoF isn't realized
        }

        public virtual void Delete(TEntity tEntity)
        {
            DbSet.Remove(tEntity);
            Save(); //ugly UoF isn't realized
        }
        private void Save()
        {
            Context.SaveChanges();

        }
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
    }
}
