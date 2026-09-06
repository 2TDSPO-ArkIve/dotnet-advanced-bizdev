using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface IFeedbackNPSUseCase
    {
        Task<IEnumerable<FeedbackNPSEntity>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<FeedbackNPSEntity?> ObterPorIdAsync(int id);

        /// <exception cref="ArgumentOutOfRangeException">Nota fora do intervalo 0-10.</exception>
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorNotaAsync(int nota, int skip = 0, int take = 50);

        Task<IEnumerable<FeedbackNPSEntity>> ObterPorResponsavelAsync(int idResponsavel, int skip = 0, int take = 50);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorAnimalAsync(int idAnimal, int skip = 0, int take = 50);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorClinicaAsync(int idClinica, int skip = 0, int take = 50);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorVeterinarioAsync(int idVeterinario, int skip = 0, int take = 50);
        Task<IEnumerable<FeedbackNPSEntity>> ObterPorDataAsync(DateTime data, int skip = 0, int take = 50);

        /// <exception cref="ArgumentException">Nenhum contexto foi informado.</exception>
        /// <exception cref="Arkive_API.Application.Exceptions.ContextoNaoEncontradoException">
        /// Algum contexto informado não existe nas tabelas externas.
        /// </exception>
        Task<FeedbackNPSEntity?> AdicionarAsync(FeedbackNPSRequestDto dto);

        // Sem EditarAsync — feedback NPS é um registro imutável por natureza.

        Task<FeedbackNPSEntity?> DeletarAsync(int id);
    }
}
