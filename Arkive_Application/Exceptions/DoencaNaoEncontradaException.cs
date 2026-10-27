namespace Arkive_Application.Exceptions
{
    /// <summary>
    /// Lançada quando uma Predisposicao referencia um IdDoenca que não existe ou está inativo.
    /// O Controller deve capturar esta exceção separadamente para retornar 404 com a mensagem,
    /// em vez de cair no catch genérico (400).
    /// </summary>
    public class DoencaNaoEncontradaException : NaoEncontradaException
    {
        public DoencaNaoEncontradaException(int idDoenca)
            : base($"Doença com ID {idDoenca} não encontrada.")
        {
        }
    }
}
