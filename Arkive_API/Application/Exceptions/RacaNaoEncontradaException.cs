namespace Arkive_API.Application.Exceptions
{
    /// <summary>
    /// Lançada quando uma Predisposicao referencia um IdRaca que não existe ou está inativo.
    /// O Controller deve capturar esta exceção separadamente para retornar 404 com a mensagem,
    /// em vez de cair no catch genérico (400).
    /// </summary>
    public class RacaNaoEncontradaException : Exception
    {
        public RacaNaoEncontradaException(int idRaca)
            : base($"Raça com ID {idRaca} não encontrada.")
        {
        }
    }
}
