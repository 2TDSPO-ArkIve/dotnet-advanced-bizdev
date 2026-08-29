namespace Arkive_API.Application.Exceptions
{
    /// <summary>
    /// Lançada quando um FeedbackNPS referencia um ID de contexto externo (Responsavel, Animal,
    /// Clinica, Consulta ou Veterinario) que não existe nas tabelas sincronizadas pela API Java.
    /// A mensagem completa é passada no construtor pois cada contexto tem uma concordância de
    /// gênero diferente em português (ex: "Consulta não encontrada" vs "Animal não encontrado").
    /// </summary>
    public class ContextoNaoEncontradoException : Exception
    {
        public ContextoNaoEncontradoException(string mensagem) : base(mensagem)
        {
        }
    }
}
