using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface IDoencaUseCase
    {
        Task<IEnumerable<DoencaEntity>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<IEnumerable<DoencaEntity>> ObterAtivasAsync(int skip = 0, int take = 50);
        Task<IEnumerable<DoencaEntity>> ObterInativasAsync(int skip = 0, int take = 50);
        Task<DoencaEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<DoencaEntity>> ObterPorNomeAsync(string nome);
        Task<IEnumerable<DoencaEntity>> ObterPorCategoriaAsync(int idCategoria);

        /// <exception cref="Arkive_API.Application.Exceptions.CategoriaNaoEncontradaException">
        /// Lançada quando dto.IdCategoria é informado mas não existe ou está inativo.
        /// </exception>
        Task<DoencaEntity?> AdicionarAsync(DoencaRequestDto dto);

        /// <exception cref="Arkive_API.Application.Exceptions.CategoriaNaoEncontradaException">
        /// Lançada quando dto.IdCategoria é informado mas não existe ou está inativo.
        /// </exception>
        Task<DoencaEntity?> EditarAsync(int id, DoencaRequestDto dto);

        Task<DoencaEntity?> ReativarAsync(int id);
        Task<DoencaEntity?> InativarAsync(int id);
    }
}
