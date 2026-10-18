using Arkive_API.Domain.Models;

namespace Arkive_API.Application
{
    /// <summary>
    /// Normaliza os parâmetros de paginação (skip/take) recebidos das consultas de lista.
    /// </summary>
    public static class Pagination
    {
        public const int TakePadrao = 50;

        /// <summary>
        /// Garante skip &gt;= 0 e take &gt;= 1, aplicando TakePadrao quando take vier zerado ou negativo.
        /// </summary>
        public static (int Skip, int Take) Normalizar(int skip, int take)
            => (Math.Max(skip, 0), take <= 0 ? TakePadrao : take);

        /// <summary>
        /// Monta os links HATEOAS da página: self sempre, next se houver mais registros, prev se skip &gt; 0.
        /// </summary>
        public static List<LinkDto> Links(string path, int skip, int take, int total)
        {
            var links = new List<LinkDto> { new($"{path}?skip={skip}&take={take}", "self") };
            if (skip + take < total) links.Add(new($"{path}?skip={skip + take}&take={take}", "next"));
            if (skip > 0) links.Add(new($"{path}?skip={Math.Max(skip - take, 0)}&take={take}", "prev"));
            return links;
        }

        /// <summary>
        /// Monta os links HATEOAS de um item: self sempre, put_{recurso} se o recurso aceita edição e delete_{recurso}.
        /// </summary>
        public static List<LinkDto> ItemLinks(string path, int id, string recurso, bool editavel = true)
        {
            var links = new List<LinkDto> { new($"{path}/{id}", "self") };
            if (editavel) links.Add(new($"{path}/{id}", $"put_{recurso}", "PUT"));
            links.Add(new($"{path}/{id}", $"delete_{recurso}", "DELETE"));
            return links;
        }
    }
}
