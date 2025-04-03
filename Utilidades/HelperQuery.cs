namespace WAPI_GS.Utilidades
{
    public static class HelperQuery
    {
        public static IQueryable<T> PaginacaoNoBanco<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), "query cannot be null");
            }
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static int GetCountTotal<T>(this IQueryable<T> query)
        {
            var queryCount = query;
            int count = queryCount.Count();
            return count;
        }
    }
}
