using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface IFeedbackNPSUseCase
    {
        Task<IEnumerable<FeedbackNPSEntity>> ObterTodosAsync();
        Task<FeedbackNPSEntity?> ObterPorIdAsync(int id);

        /// <exception cref="ArgumentOutOfRangeException">Nota fora do intervalo 0-10.</exception>
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorNotaAsync(int nota);

        Task<IEnumerable<FeedbackNPSEntity>> ObterPorResponsavelAsync(int idResponsavel);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorAnimalAsync(int idAnimal);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorClinicaAsync(int idClinica);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorVeterinarioAsync(int idVeterinario);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorDataAsync(DateTime data);

        /// <exception cref="ArgumentException">Nenhum contexto foi informado.</exception>
        /// <exception cref="Arkive_API.Application.Exceptions.ContextoNaoEncontradoException">
        /// Algum contexto informado não existe nas tabelas externas.
        /// </exception>
        Task<FeedbackNPSEntity?> AdicionarAsync(FeedbackNPSRequestDto dto);

        // Sem EditarAsync — feedback NPS é um registro imutável por natureza.

        Task<FeedbackNPSEntity?> DeletarAsync(int id);
    }
}
