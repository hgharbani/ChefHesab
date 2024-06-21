using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using Dalir.common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChefHesab.Data.Presentition.Reositories.generic
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;

        protected GenericRepository(DbContext context)
        {
            _dbContext = context;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public void Add(T entity)
        {
             _dbContext.Set<T>().Add(entity);
        }
        public async Task AddRange(List<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public virtual IList<T> SelectAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public virtual async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsQueryable().AnyAsync(predicate);
        }
        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsQueryable().Where(predicate);
        }

        public virtual IList<T> SelectAllByPage(int pageNumber, int quantity)
        {
            return _dbContext.Set<T>().Skip(Math.Max(pageNumber - 1, 0) * quantity).Take(quantity).ToList();
        }

        public virtual async Task<Tuple<int, IList<T>>> SelectDataFilteredByPage(int pageNumber, int quantity, List<Expression<Func<T, bool>>> predicate)
        {
            var Data = _dbContext.Set<T>().AsNoTracking();
            foreach (var filter in predicate)
            {
                Data = Data.WhereNullSafe(filter);
            }
            var CountData = Data.Count();
            var result = await Data.Skip(Math.Max(pageNumber - 1, 0) * quantity).Take(quantity).ToListAsync();
            return new Tuple<int, IList<T>>(CountData, result);
        }

        public virtual T Select(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().WhereNullSafe(predicate).FirstOrDefault();
        }

        public TResult Select<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> properties)
        {
            return _dbContext.Set<T>().WhereNullSafe(predicate).Select(properties).FirstOrDefault();
        }

        public IList<T> SelectList(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().WhereNullSafe(predicate).ToList();
        }
     
        public IList<TResult> SelectList<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> properties)
        {
            return _dbContext.Set<T>().WhereNullSafe(predicate).Select(properties).ToList();
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Any(predicate);
        }

     
    }
}
