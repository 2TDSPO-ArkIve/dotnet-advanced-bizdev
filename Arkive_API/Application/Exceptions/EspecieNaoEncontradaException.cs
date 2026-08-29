namespace Arkive_API.Application.Exceptions
{
    /// <summary>
    /// Lançada quando uma Raca (ou Predisposicao) referencia um IdEspecie que não existe ou está inativo.
    /// O Controller deve capturar esta exceção separadamente para retornar 404 com a mensagem,
    /// em vez de cair no catch genérico (400).
    /// </summary>
    public class EspecieNaoEncontradaException : Exception
    {
        public EspecieNaoEncontradaException(int idEspecie)
            : base($"Espécie com ID {idEspecie} não encontrada.")
        {
        }
    }
}
