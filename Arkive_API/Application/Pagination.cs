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
        /// Repassa ordenarPor/direcao nos links quando informados.
        /// </summary>
        public static List<LinkDto> Links(string path, int skip, int take, int total, string? ordenarPor = null, string? direcao = null)
        {
            var ordem = string.IsNullOrWhiteSpace(ordenarPor)
                ? ""
                : $"&ordenarPor={Uri.EscapeDataString(ordenarPor)}&direcao={Uri.EscapeDataString(direcao ?? "asc")}";
            var links = new List<LinkDto> { new($"{path}?skip={skip}&take={take}{ordem}", "self") };
            if (skip + take < total) links.Add(new($"{path}?skip={skip + take}&take={take}{ordem}", "next"));
            if (skip > 0) links.Add(new($"{path}?skip={Math.Max(skip - take, 0)}&take={take}{ordem}", "prev"));
            return links;
        }

        /// <summary>
        /// Monta os links HATEOAS de um item: self (GetById) ou get_{recurso} (lista).
        /// Ativo: put_{recurso} se o recurso aceita edição e delete_{recurso}. Inativo: só reativar_{recurso}.
        /// </summary>
        public static List<LinkDto> ItemLinks(string path, int id, string recurso, bool editavel = true, bool ativo = true, bool naLista = false)
        {
            var links = new List<LinkDto> { new($"{path}/{id}", naLista ? $"get_{recurso}" : "self") };
            if (!ativo)
            {
                links.Add(new($"{path}/reativar/{id}", $"reativar_{recurso}", "PUT"));
                return links;
            }
            if (editavel) links.Add(new($"{path}/{id}", $"put_{recurso}", "PUT"));
            links.Add(new($"{path}/{id}", $"delete_{recurso}", "DELETE"));
            return links;
        }

        /// <summary>
        /// Converte os itens de uma página mantendo deslocamento, total e links da página.
        /// </summary>
        public static PageResultModel<IEnumerable<TDto>> MapearItens<T, TDto>(PageResultModel<IEnumerable<T>> page, Func<T, TDto> map)
            => new()
            {
                Data = page.Data.Select(map).ToList(),
                Deslocamento = page.Deslocamento,
                RegistroRetornado = page.RegistroRetornado,
                TotalRegistros = page.TotalRegistros,
                Links = page.Links
            };
    }
}
