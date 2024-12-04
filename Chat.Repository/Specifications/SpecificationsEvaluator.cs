using Microsoft.EntityFrameworkCore;

namespace Chat.Repository.Specifications
{
    public class SpecificationsEvaluator<TEntity> where TEntity : class
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specs)
		{
			var query = inputQuery;

			if (specs.Criteria is not null)
				query = query.Where(specs.Criteria);

			if (specs.OrderBy is not null)
				query = query.OrderBy(specs.OrderBy);

			if (specs.OrderByDescending is not null)
				query = query.OrderBy(specs.OrderByDescending);

			if (specs.IsPaginated)
				query = query.Skip(specs.Skip).Take(specs.Take);

			query = specs.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

			return query;

		}
	}
}
