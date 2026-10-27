using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Domain.Interfaces
{
    public interface ICategoriaDoencaRepository
    {
        Task<PageResultModel<IEnumerable<CategoriaDoencaEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<CategoriaDoencaEntity>>> ObterAtivosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<CategoriaDoencaEntity>>> ObterInativosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<CategoriaDoencaEntity?> ObterPorIdAsync(int id);
        Task<CategoriaDoencaEntity?> AdicionarAsync(CategoriaDoencaEntity entity);
        Task<CategoriaDoencaEntity?> EditarAsync(int id, CategoriaDoencaEntity entity);
        Task<CategoriaDoencaEntity?> ReativarAsync(int id);
        Task<CategoriaDoencaEntity?> InativarAsync(int id);
    }
}
