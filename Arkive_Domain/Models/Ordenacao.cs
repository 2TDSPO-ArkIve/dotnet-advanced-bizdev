using System.Linq.Expressions;

namespace Arkive_Domain.Models
{
    /// <summary>
    /// Apoio à ordenação das listas paginadas (parâmetros ordenarPor/direcao).
    /// </summary>
    public static class Ordenacao
    {
        public static bool Desc(string? direcao)
            => string.Equals(direcao, "desc", StringComparison.OrdinalIgnoreCase);

        public static IOrderedQueryable<T> Por<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> chave, bool desc)
            => desc ? query.OrderByDescending(chave) : query.OrderBy(chave);
    }
}
