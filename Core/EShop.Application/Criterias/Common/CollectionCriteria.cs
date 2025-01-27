namespace EShop.Application.Criterias.Common;

public class CollectionCriteria<T> : Criteria<T>
    where T : class, new()
{
    private readonly IEnumerable<Criteria<T>> _criterias;

    public CollectionCriteria(IEnumerable<Criteria<T>> criterias)
    {
        _criterias = criterias;
    }

    public override IQueryable<T> Evaluate(IQueryable<T> query)
    {
        foreach (Criteria<T> criteria in _criterias)
            query = criteria.Evaluate(query);

        return query;
    }
}
