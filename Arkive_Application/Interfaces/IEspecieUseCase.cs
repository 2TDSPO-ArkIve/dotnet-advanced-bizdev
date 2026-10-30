using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Interfaces
{
    public interface IEspecieUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>> ObterTodasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>> ObterAtivasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>> ObterInativasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<OperationResult<GetEspecieDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<EspecieEntity?>> AdicionarAsync(EspecieRequestDto dto);
        Task<OperationResult<EspecieEntity?>> EditarAsync(int id, EspecieRequestDto dto);
        Task<OperationResult<EspecieEntity?>> ReativarAsync(int id);
        Task<OperationResult<EspecieEntity?>> InativarAsync(int id);
    }
}
