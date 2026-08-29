namespace Arkive_API.Application.Exceptions
{
    /// <summary>
    /// Lançada quando uma Doenca referencia um IdCategoria que não existe ou está inativo.
    /// O Controller deve capturar esta exceção separadamente para retornar 404 com a mensagem,
    /// em vez de cair no catch genérico (400).
    /// </summary>
    public class CategoriaNaoEncontradaException : Exception
    {
        public CategoriaNaoEncontradaException(int idCategoria)
            : base($"Categoria com ID {idCategoria} não encontrada.")
        {
        }
    }
}
