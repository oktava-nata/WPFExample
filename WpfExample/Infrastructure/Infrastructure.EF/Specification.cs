using System;
using System.Linq.Expressions;
using Infrastructure.Domain;

namespace Infrastructure.EF
{
    public class Specification<T> : ISpecification<T>
    {
        private Func<T, bool> _function;

        private Func<T, bool> Function => _function
            ?? (_function = Predicate.Compile());

        protected Expression<Func<T, bool>> Predicate;

        protected Specification() { }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Function.Invoke(entity);
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return Predicate;
        }
    }
}
