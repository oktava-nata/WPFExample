using System;
using System.Linq.Expressions;

namespace Infrastructure.Domain
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
        Expression<Func<T, bool>> ToExpression();
    }
}
