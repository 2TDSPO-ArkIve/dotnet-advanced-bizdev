using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IEspecieUseCase
    {
        Task<OperationResult<IEnumerable<EspecieEntity>>> ObterTodasAsync();
        Task<OperationResult<IEnumerable<EspecieEntity>>> ObterAtivasAsync();
        Task<OperationResult<IEnumerable<EspecieEntity>>> ObterInativasAsync();
        Task<OperationResult<GetEspecieDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<EspecieEntity?>> AdicionarAsync(EspecieRequestDto dto);
        Task<OperationResult<EspecieEntity?>> EditarAsync(int id, EspecieRequestDto dto);
        Task<OperationResult<EspecieEntity?>> ReativarAsync(int id);
        Task<OperationResult<EspecieEntity?>> InativarAsync(int id);
    }
}
