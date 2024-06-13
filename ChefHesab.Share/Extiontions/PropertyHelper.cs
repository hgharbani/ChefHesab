using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Share.Extiontions
{
    public class PropertyHelper : ExpressionVisitor
    {
        private Stack<string> _stack;

        //
        // Parameters:
        //   expression:
        public string GetNestedPropertyPath(Expression expression)
        {
            _stack = new Stack<string>();
            Visit(expression);
            return _stack.Aggregate((string s1, string s2) => s1 + "." + s2);
        }

        //
        // Parameters:
        //   expression:
        protected override Expression VisitMember(MemberExpression expression)
        {
            if (_stack != null)
            {
                _stack.Push(expression.Member.Name);
            }

            return base.VisitMember(expression);
        }

        //
        // Parameters:
        //   expression:
        //
        // Type parameters:
        //   TEntity:
        public string GetNestedPropertyName<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return GetNestedPropertyPath(expression);
        }
    }
}
