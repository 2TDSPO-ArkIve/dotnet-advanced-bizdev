using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IRacaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<RacaEntity>>>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<RacaEntity>>>> ObterAtivasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<RacaEntity>>>> ObterInativasAsync(int skip = 0, int take = 50);
        Task<OperationResult<GetRacaDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<IEnumerable<RacaEntity>>> ObterPorEspecieAsync(int idEspecie);

        /// <summary>
        /// Retorna Failure 404 quando dto.IdEspecie não existe ou está inativo.
        /// </summary>
        Task<OperationResult<RacaEntity?>> AdicionarAsync(RacaRequestDto dto);

        /// <summary>
        /// Retorna Failure 404 quando a raça não existe/está inativa ou quando dto.IdEspecie
        /// não existe ou está inativo.
        /// </summary>
        Task<OperationResult<RacaEntity?>> EditarAsync(int id, RacaRequestDto dto);

        Task<OperationResult<RacaEntity?>> ReativarAsync(int id);
        Task<OperationResult<RacaEntity?>> InativarAsync(int id);
    }
}
