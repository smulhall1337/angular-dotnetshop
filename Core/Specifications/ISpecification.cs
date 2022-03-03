using System.Linq.Expressions;

namespace Core.Specifications
{
    // Specifications can be used to build Iqueryables of Generic repositorys 
    // to pass in more arguments that would otherwise be very difficult in a plain old generic repository
    // i.e. we want all products but only products that have the string "red" in the name
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; } // this will be our 'where...' criteria
        List<Expression<Func<T, object>>> Includes { get; }// this will be our 'includes...' criteria
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        // pagination
        int Take { get; } // how many we're displaying
        int Skip { get; } // how many we're skipping to get to the page we want
        bool IsPagingEnabled { get; }
    }
}
