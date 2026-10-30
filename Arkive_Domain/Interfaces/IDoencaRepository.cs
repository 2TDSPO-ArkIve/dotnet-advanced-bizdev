using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Domain.Interfaces
{
    public interface IDoencaRepository
    {
        Task<PageResultModel<IEnumerable<DoencaEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<DoencaEntity>>> ObterAtivosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<DoencaEntity>>> ObterInativosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<DoencaEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<DoencaEntity>> ObterPorNomeAsync(string nome);
        Task<IEnumerable<DoencaEntity>> ObterPorCategoriaAsync(int idCategoria);
        Task<DoencaEntity?> AdicionarAsync(DoencaEntity entity);
        Task<DoencaEntity?> EditarAsync(int id, DoencaEntity entity);
        Task<DoencaEntity?> ReativarAsync(int id);
        Task<DoencaEntity?> InativarAsync(int id);
    }
}
