
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository
{

        public  interface IBaseGenericRepository<TEntity> : IDisposable where TEntity : class
        {
            void Insert(TEntity entity);

            void Insert(IEnumerable<TEntity> entities);

            void Update(TEntity entity);

            void Update(IEnumerable<TEntity> entities);

            void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propriedades);

            void Delete(TEntity entity);

            void Delete(IEnumerable<TEntity> entities);

            void Delete(Expression<Func<TEntity, bool>> predicate);

            TEntity SelectByKey(params object[] primaryKeys);

            IList<TEntity> SelectAll();

            IList<TEntity> SelectAllByPage(int pageNumber, int quantity);

            TEntity Select(Expression<Func<TEntity, bool>> predicate);

            TResult Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> properties);
        }

    
}
