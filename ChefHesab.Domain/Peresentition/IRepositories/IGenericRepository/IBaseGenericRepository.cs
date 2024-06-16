
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);


        IList<T> SelectAll();

        IList<T> SelectAllByPage(int pageNumber, int quantity);

        T Select(Expression<Func<T, bool>> predicate);

        TResult Select<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> properties);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<Tuple<int, IList<T>>> SelectDataFilteredByPage(int pageNumber, int quantity, List<Expression<Func<T, bool>>> predicate);
        Task AddRange(List<T> entity);
    }


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
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
         Task<Tuple<int, IList<TEntity>>> SelectDataFilteredByPage(int pageNumber, int quantity, List<Expression<Func<TEntity, bool>>> predicate);
    }

    
}
