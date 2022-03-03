using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // using TEntity (can be anything) to specify that we'll be using this specifically for 
    // classes that derive BaseEntity
    internal class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
            ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
                // in this case, our criteria could be something like (p => p.ProductTypeId == id)
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled)
            {
                // paging needs to come after filtering 
                // we wouldn't want to get everything in that page and then apply the filtering
                query = query.Skip(specification.Skip).Take(specification.Take);
            }


            // appends an include() statement to our query
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
