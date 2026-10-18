using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface ICategoriaDoencaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<CategoriaDoencaEntity>>>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<CategoriaDoencaEntity>>>> ObterAtivasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<CategoriaDoencaEntity>>>> ObterInativasAsync(int skip = 0, int take = 50);
        Task<OperationResult<GetCategoriaDoencaDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<CategoriaDoencaEntity?>> AdicionarAsync(CategoriaDoencaRequestDto dto);
        Task<OperationResult<CategoriaDoencaEntity?>> EditarAsync(int id, CategoriaDoencaRequestDto dto);
        Task<OperationResult<CategoriaDoencaEntity?>> ReativarAsync(int id);
        Task<OperationResult<CategoriaDoencaEntity?>> InativarAsync(int id);
    }
}
