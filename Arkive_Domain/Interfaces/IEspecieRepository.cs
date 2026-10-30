using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Domain.Interfaces
{
    public interface IEspecieRepository
    {
        Task<PageResultModel<IEnumerable<EspecieEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<EspecieEntity>>> ObterAtivosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<EspecieEntity>>> ObterInativosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<EspecieEntity?> ObterPorIdAsync(int id);
        Task<EspecieEntity?> AdicionarAsync(EspecieEntity entity);
        Task<EspecieEntity?> EditarAsync(int id, EspecieEntity entity);
        Task<EspecieEntity?> ReativarAsync(int id);
        Task<EspecieEntity?> InativarAsync(int id);
    }
}
