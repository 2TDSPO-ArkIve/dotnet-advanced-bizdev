using Arkive_API.Domain.Entities;

namespace Arkive_API.Domain.Interfaces
{
    public interface IEspecieRepository
    {
        Task<IEnumerable<EspecieEntity>> ObterTodosAsync();
        Task<IEnumerable<EspecieEntity>> ObterAtivosAsync();
        Task<IEnumerable<EspecieEntity>> ObterInativosAsync();
        Task<EspecieEntity?> ObterPorIdAsync(int id);
        Task<EspecieEntity?> AdicionarAsync(EspecieEntity entity);
        Task<EspecieEntity?> EditarAsync(int id, EspecieEntity entity);
        Task<EspecieEntity?> ReativarAsync(int id);
        Task<EspecieEntity?> InativarAsync(int id);
    }
}
