namespace Arkive_API.Application
{
    /// <summary>
    /// Normaliza os parâmetros de paginação (skip/take) recebidos das consultas de lista.
    /// </summary>
    public static class Pagination
    {
        public const int TakePadrao = 50;
        public const int TakeMaximo = 100;

        /// <summary>
        /// Garante skip &gt;= 0 e take dentro do intervalo [1, TakeMaximo].
        /// </summary>
        public static (int Skip, int Take) Normalizar(int skip, int take)
            => (Math.Max(skip, 0), Math.Clamp(take, 1, TakeMaximo));
    }
}
