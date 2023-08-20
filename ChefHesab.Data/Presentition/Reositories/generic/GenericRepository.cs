using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using Dalir.common.Context;
using Dalir.common.Extensions;
using Dalir.common.Interfaces;
using Dalir.common.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Data.Presentition.Reositories.generic
{
    public  class GenericRepository<TEntity> : IBaseGenericRepository<TEntity>, IDisposable where TEntity : class
    {
        protected readonly ChefHesabContext _dbContext;

        public DbSet<TEntity> DbSet;

        public GenericRepository(ChefHesabContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");

            try
            {
                DbSet = dbContext.Set<TEntity>();
            }
            catch (InvalidOperationException innerException)
            {
                throw new Exception(string.Format(Errors.Possible_0, "map the entity: " + GetType().Name), innerException);
            }
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propriedades)
        {
            _dbContext.Attach(entity);
            foreach (Expression<Func<TEntity, object>> item in propriedades.AsParallel())
            {
                _dbContext.Entry(entity).Property(item).IsModified = true;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            DbSet.AsQueryable().Where(predicate).ToList()
                .ForEach(delegate (TEntity entity)
                {
                    DbSet.Remove(entity);
                });
        }

        public TEntity SelectByKey(params object[] primaryKeys)
        {
            return DbSet.Find(primaryKeys);
        }

        public virtual IList<TEntity> SelectAll()
        {
            return DbSet.ToList();
        }

        public virtual IList<TEntity> SelectAllByPage(int pageNumber, int quantity)
        {
            return DbSet.Skip(Math.Max(pageNumber - 1, 0) * quantity).Take(quantity).ToList();
        }

        public virtual TEntity Select(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.WhereNullSafe(predicate).FirstOrDefault();
        }

        public TResult Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> properties)
        {
            return DbSet.WhereNullSafe(predicate).Select(properties).FirstOrDefault();
        }

        public IList<TEntity> SelectList(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.WhereNullSafe(predicate).ToList();
        }

        public IList<TResult> SelectList<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> properties)
        {
            return DbSet.WhereNullSafe(predicate).Select(properties).ToList();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
