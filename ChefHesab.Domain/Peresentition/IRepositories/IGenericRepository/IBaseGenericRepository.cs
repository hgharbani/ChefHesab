
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
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);


        IList<T> SelectAll();

        IList<T> SelectAllByPage(int pageNumber, int quantity);

        T Select(Expression<Func<T, bool>> predicate);

        TResult Select<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> properties);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<Tuple<int, IList<T>>> SelectDataFilteredByPage(int pageNumber, int quantity, List<Expression<Func<T, bool>>> predicate);
        Task AddRange(List<T> entity);
  

        IQueryable<T> SelectQuery();
        double CalculateLeaveDays(string formulaString, Dictionary<string, object> formulsParametes);
    }


    
}
