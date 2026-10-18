using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IEspecieUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<EspecieEntity>>>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<EspecieEntity>>>> ObterAtivasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<EspecieEntity>>>> ObterInativasAsync(int skip = 0, int take = 50);
        Task<OperationResult<GetEspecieDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<EspecieEntity?>> AdicionarAsync(EspecieRequestDto dto);
        Task<OperationResult<EspecieEntity?>> EditarAsync(int id, EspecieRequestDto dto);
        Task<OperationResult<EspecieEntity?>> ReativarAsync(int id);
        Task<OperationResult<EspecieEntity?>> InativarAsync(int id);
    }
}
