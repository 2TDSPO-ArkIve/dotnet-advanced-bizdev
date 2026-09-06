using Arkive_API.Domain.Entities;

namespace Arkive_API.Domain.Interfaces
{
    public interface IDoencaRepository
    {
        Task<IEnumerable<DoencaEntity>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<IEnumerable<DoencaEntity>> ObterAtivosAsync(int skip = 0, int take = 50);
        Task<IEnumerable<DoencaEntity>> ObterInativosAsync(int skip = 0, int take = 50);
        Task<DoencaEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<DoencaEntity>> ObterPorNomeAsync(string nome);
        Task<IEnumerable<DoencaEntity>> ObterPorCategoriaAsync(int idCategoria);
        Task<DoencaEntity?> AdicionarAsync(DoencaEntity entity);
        Task<DoencaEntity?> EditarAsync(int id, DoencaEntity entity);
        Task<DoencaEntity?> ReativarAsync(int id);
        Task<DoencaEntity?> InativarAsync(int id);
    }
}
