using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Domain.Interfaces
{
    public interface IRacaRepository
    {
        Task<PageResultModel<IEnumerable<RacaEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<RacaEntity>>> ObterAtivosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<RacaEntity>>> ObterInativosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<RacaEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<RacaEntity>> ObterPorEspecieAsync(int idEspecie);
        Task<RacaEntity?> AdicionarAsync(RacaEntity entity);
        Task<RacaEntity?> EditarAsync(int id, RacaEntity entity);
        Task<RacaEntity?> ReativarAsync(int id);
        Task<RacaEntity?> InativarAsync(int id);
    }
}
