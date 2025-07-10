using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using ChefHesab.Share;
using Dalir.common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ChefHesab.Data.Presentition.Reositories.generic
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext _dbContext { get; private set; }

        public GenericRepository(ChefHesabContext chefHesab)
        {
           
            _dbContext = chefHesab;
        }


       

        public double CalculateLeaveDays(string formulaString, Dictionary<string, object> formulsParametes)
        {
            var tableName = "AdditionalCosts";
            var columnName = "Price";

            // ایجاد عبارت
             formulaString = $"{tableName}_{columnName} * 1000 / 1000";
            var expression = new NCalc.Expression(formulaString);

            // تعیین مقدار پارامترها (به جای این قسمت، از منطق دسترسی به پایگاه داده خود استفاده کنید)
            expression.Parameters[tableName + "_" + columnName] = GetValueFromTable(tableName, columnName);

            // ارزیابی عبارت
  


            //var formula = _context.Formulas.Include(f => f.Parameters).SingleOrDefault(f => f.Id == formulaId);

            // ایجاد یک شیء از کلاس Expression از NCalc
            //var expression = new NCalc.Expression(formulaString);
            var x = expression.Parameters.Keys.ToList();

            // تنظیم پارامترهای عبارت
            foreach (var parameter in formulsParametes)
            {
                expression.Parameters[parameter.Key] = parameter.Value;
            }

            // ارزیابی عبارت و بازگرداندن نتیجه
            var result = expression.Evaluate();
            return Convert.ToDouble(result);





            //// بازیابی اطلاعات پرسنل از دیتابیس


            //// ایجاد یک شیء Expression از NCalc
            //var expression = new NCalc.Expression(formulaString);
            //var parameters = expression.Parameters.Keys.ToList();
            //Regex parameterRegex = new Regex(@"[%)(/*+-]\w*");

            //// پیدا کردن پارامترها
            //MatchCollection matches = parameterRegex.Matches(formulaString);
            //List<string> parameters1 = matches.Cast<Match>().Select(m => m.Value).ToList();

            //// جایگزینی پارامترها با نگهدارنده‌ها
            //string modifiedFormula = Regex.Replace(formulaString, parameterRegex.ToString(), "{@}");
            //var listParameters = modifiedFormula.Split("{@}").ToList();
            //foreach (var parameters in listParameters)
            //{
            //    if (IsNumeric(parameters)) continue;
            //    expression.Parameters[parameters] = GetFieldValue<dynamic>(parameters, formulsParametes);
            //}


            //// ارزیابی عبارت و بازگرداندن نتیجه
            //object result = expression.Evaluate();
            //return Convert.ToDouble(result);
        }
        // تابع برای دریافت مقدار از جدول
        private decimal GetValueFromTable(string tableName, string columnName)
        {
           
                // بررسی اعتبار ورودی‌ها
                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(columnName))
                {
                    throw new ArgumentException("نام جدول یا ستون نمی‌تواند خالی باشد.");
                }

                // استفاده از Reflection برای دسترسی دینامیک به DbSet
                var dbSetProperty = _dbContext.GetType().GetProperty(tableName);
                if (dbSetProperty == null)
                {
                    throw new ArgumentException($"جدول {tableName} در DbContext یافت نشد.");
                }

                var dbSet = dbSetProperty.GetValue(_dbContext) ;
                if (dbSet == null)
                {
                    throw new ArgumentException($"نوع DbSet برای جدول {tableName} صحیح نیست.");
                }
                var AdditionalCosts= dbSet.GetType().GetNestedType(columnName);
            //// استفاده از LINQ برای انتخاب مقدار
            //Expression<Func<object, object>> selector = x => x.GetType().GetProperty(columnName).GetValue(x);
            //var value = dbSet.Select(selector)
            //    .FirstOrDefault();

            //    if (value == null)
            //    {
            //        throw new ArgumentException($"ستون {columnName} در جدول {tableName} یافت نشد یا مقداری ندارد.");
            //    }

                return Convert.ToDecimal(0);
            
        }
        public decimal Evaluate(string formulaString, Dictionary<string, object> parameters)
        {
            //var formula = _context.Formulas.Include(f => f.Parameters).SingleOrDefault(f => f.Id == formulaId);

            // ایجاد یک شیء از کلاس Expression از NCalc
            var expression = new NCalc.Expression(formulaString);
           var x= expression.Parameters.Keys.ToList();
            // تنظیم پارامترهای عبارت
            foreach (var parameter in parameters)
            {
                expression.Parameters[parameter.Key] = parameter.Value;
            }

            // ارزیابی عبارت و بازگرداندن نتیجه
            var result = expression.Evaluate();
            return Convert.ToDecimal(result);
        }

        bool IsNumeric(string str)
        {
            int number;
            return int.TryParse(str, out number);
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

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
  
            return await _dbContext.Set<T>().AsQueryable().AnyAsync(predicate);
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {

            return  _dbContext.Set<T>().AsQueryable().Any(predicate);
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
        public IQueryable<T> SelectQuery()
        {
            return _dbContext.Set<T>();
        }


    }
}
