using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Interfaces
{
    public interface IPredisposicaoUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterTodasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<GetPredisposicaoDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");

        /// <summary>
        /// Retorna Failure 404 quando dto.IdEspecie, dto.IdRaca (se informado) ou dto.IdDoenca
        /// não existe ou está inativo.
        /// </summary>
        Task<OperationResult<PredisposicaoEntity?>> AdicionarAsync(PredisposicaoRequestDto dto);

        // Sem EditarAsync — conforme PRD §16.3.5, o PUT foi substituído por remover e recriar o vínculo.

        Task<OperationResult<PredisposicaoEntity?>> DeletarAsync(int id);
    }
}
