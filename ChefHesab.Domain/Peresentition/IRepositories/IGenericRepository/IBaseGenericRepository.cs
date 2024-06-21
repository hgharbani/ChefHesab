
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllQuaryble();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);

        Task<TEntity> GetById(int id);
        Task<IEnumerable<TEntity>> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);


        IList<TEntity> SelectAll();

        IList<TEntity> SelectAllByPage(int pageNumber, int quantity);

        TEntity Select(Expression<Func<TEntity, bool>> predicate);

        TResult Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> properties);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Tuple<int, IList<TEntity>>> SelectDataFilteredByPage(int pageNumber, int quantity, List<Expression<Func<TEntity, bool>>> predicate);
        Task AddRange(List<TEntity> entity);
    }


    
}
