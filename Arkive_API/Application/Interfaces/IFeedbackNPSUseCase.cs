using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IFeedbackNPSUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<OperationResult<GetFeedbackNPSDto?>> ObterPorIdAsync(int id);

        /// <summary>
        /// Retorna Failure 400 quando a nota está fora do intervalo 0-5.
        /// </summary>
        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterPorNotaAsync(int nota, int skip = 0, int take = 50);

        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterPorResponsavelAsync(int idResponsavel, int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterPorAnimalAsync(int idAnimal, int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterPorClinicaAsync(int idClinica, int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterPorVeterinarioAsync(int idVeterinario, int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<FeedbackNPSEntity>>>> ObterPorDataAsync(DateTime data, int skip = 0, int take = 50);

        /// <summary>
        /// Retorna Failure 400 quando nenhum contexto é informado e Failure 404 quando algum
        /// contexto informado não existe nas tabelas externas.
        /// </summary>
        Task<OperationResult<FeedbackNPSEntity?>> AdicionarAsync(FeedbackNPSRequestDto dto);

        // Sem EditarAsync — feedback NPS é um registro imutável por natureza.

        Task<OperationResult<FeedbackNPSEntity?>> DeletarAsync(int id);
    }
}
