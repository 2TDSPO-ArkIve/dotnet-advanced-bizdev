using Arkive_API.Domain.Entities;

namespace Arkive_API.Domain.Interfaces
{
    public interface ICategoriaDoencaRepository
    {
        Task<IEnumerable<CategoriaDoencaEntity>> ObterTodosAsync();
        Task<IEnumerable<CategoriaDoencaEntity>> ObterAtivosAsync();
        Task<IEnumerable<CategoriaDoencaEntity>> ObterInativosAsync();
        Task<CategoriaDoencaEntity?> ObterPorIdAsync(int id);
        Task<CategoriaDoencaEntity?> AdicionarAsync(CategoriaDoencaEntity entity);
        Task<CategoriaDoencaEntity?> EditarAsync(int id, CategoriaDoencaEntity entity);
        Task<CategoriaDoencaEntity?> ReativarAsync(int id);
        Task<CategoriaDoencaEntity?> InativarAsync(int id);
    }
}
