using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Interfaces
{
    public interface IRacaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<GetRacaDto>>>> ObterTodasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<PageResultModel<IEnumerable<GetRacaDto>>>> ObterAtivasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<PageResultModel<IEnumerable<GetRacaDto>>>> ObterInativasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<GetRacaDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<IEnumerable<GetRacaDto>>> ObterPorEspecieAsync(int idEspecie);

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
