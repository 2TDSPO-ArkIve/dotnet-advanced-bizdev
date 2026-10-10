using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface ICategoriaDoencaUseCase
    {
        Task<OperationResult<IEnumerable<CategoriaDoencaEntity>>> ObterTodasAsync();
        Task<OperationResult<IEnumerable<CategoriaDoencaEntity>>> ObterAtivasAsync();
        Task<OperationResult<IEnumerable<CategoriaDoencaEntity>>> ObterInativasAsync();
        Task<OperationResult<CategoriaDoencaEntity?>> ObterPorIdAsync(int id);
        Task<OperationResult<CategoriaDoencaEntity?>> AdicionarAsync(CategoriaDoencaRequestDto dto);
        Task<OperationResult<CategoriaDoencaEntity?>> EditarAsync(int id, CategoriaDoencaRequestDto dto);
        Task<OperationResult<CategoriaDoencaEntity?>> ReativarAsync(int id);
        Task<OperationResult<CategoriaDoencaEntity?>> InativarAsync(int id);
    }
}
