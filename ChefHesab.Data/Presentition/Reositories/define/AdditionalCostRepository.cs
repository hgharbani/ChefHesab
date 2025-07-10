using ChefHesab.Data;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ChefHesab.Domain.Peresentition.IRepositories
{
    /// <summary>
    /// سرویس مدیریت هزینه های مازاد
    /// </summary>
    public class AdditionalCostRepository : GenericRepository<AdditionalCost>, IAdditionalCostRepository
    {

        public AdditionalCostRepository(ChefHesabContext chefHesab) : base(chefHesab)
        {

        }
    
        public IQueryable<AdditionalCost> GetAll()
        {
            return _dbContext.Set<AdditionalCost>().AsQueryable();
        }
        public IQueryable<AdditionalCost> GetAllAsNoTracking()
        {
            return _dbContext.Set<AdditionalCost>().AsQueryable().AsNoTracking();
        }

        private FieldInfos ParseFieldName(string parameterName)
        {
            // فرض کنید جداکننده بین نام جدول و فیلد نقطه (.) باشد
            var parts = parameterName.Split('.');
            return new FieldInfos { TableName = parts[0], FieldName = parts[1], FieldCondition = parts.Count() > 2 ? parts[2] : "" };
        }

        private dynamic GetPropertyValue(object obj, string propertyName, List<FormulsParametes> formulsParametes)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
        }

        private object GetFieldValue<T>(string parameterName, List<FormulsParametes> formulsParametes) where T : class
        {
            var fieldInfo = ParseFieldName(parameterName);

            // استفاده از Entity Framework برای دسترسی به دیتابیس
            var type = Type.GetType($"ChefHesab.Domain.{fieldInfo.TableName}");
            if (type == null)
            {
                throw new ArgumentException("Invalid table name");
            }

            // ایجاد DbSet برای نوع پویا
            var table = _dbContext.Set<T>(fieldInfo.TableName);

            // مثال: گرفتن اولین رکورد از جدول
            var result = table.FirstOrDefault();
            var property = GetPropertyValue(result, fieldInfo.FieldName, formulsParametes);
            return property;

        }

        private double CalculateLeaveDays(string formulaString, List<FormulsParametes> formulsParametes)
        {
            // بازیابی اطلاعات پرسنل از دیتابیس


            // ایجاد یک شیء Expression از NCalc
            var expression = new NCalc.Expression(formulaString);

            Regex parameterRegex = new Regex(@"[%)(/*+-]\w*");

            // پیدا کردن پارامترها
            MatchCollection matches = parameterRegex.Matches(formulaString);
            List<string> parameters1 = matches.Cast<Match>().Select(m => m.Value).ToList();

            // جایگزینی پارامترها با نگهدارنده‌ها
            string modifiedFormula = Regex.Replace(formulaString, parameterRegex.ToString(), "{@}");
            var listParameters = modifiedFormula.Split("{@}").ToList();
            foreach (var parameters in listParameters)
            {
                if (IsNumeric(parameters)) continue;
                expression.Parameters[parameters] = GetFieldValue<dynamic>(parameters, formulsParametes);
            }


            // ارزیابی عبارت و بازگرداندن نتیجه
            object result = expression.Evaluate();
            return Convert.ToDouble(result);
        }
        bool IsNumeric(string str)
        {
            int number;
            return int.TryParse(str, out number);
        }

    }
}