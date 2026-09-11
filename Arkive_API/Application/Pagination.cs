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
    }
}
