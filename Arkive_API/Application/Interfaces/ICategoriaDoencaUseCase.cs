using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface ICategoriaDoencaUseCase
    {
        Task<IEnumerable<CategoriaDoencaEntity>> ObterTodasAsync();
        Task<IEnumerable<CategoriaDoencaEntity>> ObterAtivasAsync();
        Task<IEnumerable<CategoriaDoencaEntity>> ObterInativasAsync();
        Task<CategoriaDoencaEntity?> ObterPorIdAsync(int id);
        Task<CategoriaDoencaEntity?> AdicionarAsync(CategoriaDoencaRequestDto dto);
        Task<CategoriaDoencaEntity?> EditarAsync(int id, CategoriaDoencaRequestDto dto);
        Task<CategoriaDoencaEntity?> ReativarAsync(int id);
        Task<CategoriaDoencaEntity?> InativarAsync(int id);
    }
}
