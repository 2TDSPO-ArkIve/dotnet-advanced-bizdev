using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface IEspecieUseCase
    {
        Task<IEnumerable<EspecieEntity>> ObterTodasAsync();
        Task<IEnumerable<EspecieEntity>> ObterAtivasAsync();
        Task<IEnumerable<EspecieEntity>> ObterInativasAsync();
        Task<EspecieEntity?> ObterPorIdAsync(int id);
        Task<EspecieEntity?> AdicionarAsync(EspecieRequestDto dto);
        Task<EspecieEntity?> EditarAsync(int id, EspecieRequestDto dto);
        Task<EspecieEntity?> ReativarAsync(int id);
        Task<EspecieEntity?> InativarAsync(int id);
    }
}
