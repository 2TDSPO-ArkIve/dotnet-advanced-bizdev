using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IDoencaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>> ObterAtivasAsync(int skip = 0, int take = 50);
        Task<OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>> ObterInativasAsync(int skip = 0, int take = 50);
        Task<OperationResult<GetDoencaDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<IEnumerable<GetDoencaDto>>> ObterPorNomeAsync(string nome);
        Task<OperationResult<IEnumerable<GetDoencaDto>>> ObterPorCategoriaAsync(int idCategoria);

        /// <summary>
        /// Retorna Failure 404 quando dto.IdCategoria é informado mas não existe ou está inativo.
        /// </summary>
        Task<OperationResult<DoencaEntity?>> AdicionarAsync(DoencaRequestDto dto);

        /// <summary>
        /// Retorna Failure 404 quando a doença não existe/está inativa ou quando dto.IdCategoria
        /// é informado mas não existe ou está inativo.
        /// </summary>
        Task<OperationResult<DoencaEntity?>> EditarAsync(int id, DoencaRequestDto dto);

        Task<OperationResult<DoencaEntity?>> ReativarAsync(int id);
        Task<OperationResult<DoencaEntity?>> InativarAsync(int id);
    }
}
