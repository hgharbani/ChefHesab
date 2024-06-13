using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Share.Extiontions
{
    public static class PropertyExtensions
    {
        //
        // Parameters:
        //   expression:
        //
        // Type parameters:
        //   T:
        public static MemberInfo GetMember<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member;
            }

            if (expression.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Expression is not a MemberExpression", "expression");
            }

            return memberExpression.Member;
        }

        //
        // Parameters:
        //   expression:
        //
        // Type parameters:
        //   T:
        public static string PropertyName<T>(this Expression<Func<T, object>> expression)
        {
            return new PropertyHelper().GetNestedPropertyName(expression);
        }

        //
        // Parameters:
        //   expression:
        //
        // Type parameters:
        //   T:
        public static string PropertyDisplay<T>(this Expression<Func<T, object>> expression)
        {
            MemberInfo member = expression.GetMember();
            object[] customAttributes = member.GetCustomAttributes(typeof(DisplayNameAttribute), inherit: true);
            if (customAttributes.Length != 1)
            {
                return member.Name;
            }

            return ((DisplayNameAttribute)customAttributes[0]).DisplayName;
        }

        //
        // Parameters:
        //   source:
        //
        //   propertyLambda:
        //
        // Type parameters:
        //   TSource:
        //
        //   TProperty:
        public static string GetPropertyInfo<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type typeFromHandle = typeof(TSource);
            if (!(propertyLambda.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            if (propertyInfo.ReflectedType != null && typeFromHandle != propertyInfo.ReflectedType && !typeFromHandle.IsSubclassOf(propertyInfo.ReflectedType))
            {
                throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type {typeFromHandle}.");
            }

            return propertyInfo.Name;
        }
    }
}
