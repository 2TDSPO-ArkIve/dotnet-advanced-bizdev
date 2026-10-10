using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IPredisposicaoUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<PredisposicaoEntity>>>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PredisposicaoEntity?>> ObterPorIdAsync(int id);
        Task<OperationResult<PageResultModel<IEnumerable<PredisposicaoEntity>>>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<PredisposicaoEntity>>>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<PredisposicaoEntity>>>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50);

        /// <summary>
        /// Retorna Failure 404 quando dto.IdEspecie, dto.IdRaca (se informado) ou dto.IdDoenca
        /// não existe ou está inativo.
        /// </summary>
        Task<OperationResult<PredisposicaoEntity?>> AdicionarAsync(PredisposicaoRequestDto dto);

        // Sem EditarAsync — conforme PRD §16.3.5, o PUT foi substituído por remover e recriar o vínculo.

        Task<OperationResult<PredisposicaoEntity?>> DeletarAsync(int id);
    }
}
