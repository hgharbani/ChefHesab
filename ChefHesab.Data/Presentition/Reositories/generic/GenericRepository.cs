using Dalir.common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using ChefHesab.Data.Presentition.Context;
namespace ChefHesab.Data.Presentition.Reositories.generic
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ChefHesabContext _Context;

        public GenericRepository(ChefHesabContext Context)
        {
            _Context = Context;
        }



        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _Context.Set<TEntity>().ToListAsync();
        }
        public IQueryable<TEntity> GetAllQueryable()
        {
            return  _Context.Set<TEntity>().AsQueryable();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _Context.Set<TEntity>().Update(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _Context.Set<TEntity>().Remove(entity);
        }
        public async Task<TEntity> GetById(int id)
        {
            return await _Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _Context.Set<TEntity>().ToListAsync();
        }

        public void Add(TEntity entity)
        {
             _Context.Set<TEntity>().AddAsync(entity);
        }
        public async Task AddRange(List<TEntity> entity)
        {
            await _Context.Set<TEntity>().AddRangeAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _Context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _Context.Set<TEntity>().Update(entity);
        }

        public virtual IList<TEntity> SelectAll()
        {
            return _Context.Set<TEntity>().ToList();
        }

        public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await _Context.Set<TEntity>().AsQueryable().AnyAsync(predicate);
        }
        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _Context.Set<TEntity>().AsQueryable().Where(predicate);
        }

        public virtual IList<TEntity> SelectAllByPage(int pageNumber, int quantity)
        {
            return _Context.Set<TEntity>().Skip(Math.Max(pageNumber - 1, 0) * quantity).Take(quantity).ToList();
        }

        public virtual async Task<Tuple<int, IList<TEntity>>> SelectDataFilteredByPage(int pageNumber, int quantity, List<Expression<Func<TEntity, bool>>> predicate)
        {
            var Data = _Context.Set<TEntity>().AsNoTracking();
            foreach (var filter in predicate)
            {
                Data = Data.WhereNullSafe(filter);
            }
            var CountData = Data.Count();
            var result = await Data.Skip(Math.Max(pageNumber - 1, 0) * quantity).Take(quantity).ToListAsync();
            return new Tuple<int, IList<TEntity>>(CountData, result);
        }

        public virtual TEntity Select(Expression<Func<TEntity, bool>> predicate)
        {
            return _Context.Set<TEntity>().WhereNullSafe(predicate).FirstOrDefault();
        }

        public TResult Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> properties)
        {
            return _Context.Set<TEntity>().WhereNullSafe(predicate).Select(properties).FirstOrDefault();
        }

        public IList<TEntity> SelectList(Expression<Func<TEntity, bool>> predicate)
        {
            return _Context.Set<TEntity>().WhereNullSafe(predicate).ToList();
        }

        public IList<TResult> SelectList<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> properties)
        {
            return _Context.Set<TEntity>().WhereNullSafe(predicate).Select(properties).ToList();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _Context.Set<TEntity>().Any(predicate);
        }

        public Task<IQueryable<TEntity>> GetAllQuaryble()
        {
            throw new NotImplementedException();
        }

        TEntity IGenericRepository<TEntity>.Select(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
