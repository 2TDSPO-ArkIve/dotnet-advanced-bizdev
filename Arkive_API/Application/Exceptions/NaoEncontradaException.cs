namespace Arkive_API.Application.Exceptions
{
    /// <summary>
    /// Base das exceções de "recurso referenciado não encontrado". O handler global
    /// (<see cref="Arkive_API.Presentation.Handlers.GlobalExceptionHandler"/>) as traduz em 404.
    /// </summary>
    public abstract class NaoEncontradaException : Exception
    {
        protected NaoEncontradaException(string mensagem) : base(mensagem)
        {
        }
    }
}
