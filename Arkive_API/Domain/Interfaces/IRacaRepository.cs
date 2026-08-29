using Arkive_API.Domain.Entities;

namespace Arkive_API.Domain.Interfaces
{
    public interface IRacaRepository
    {
        Task<IEnumerable<RacaEntity>> ObterTodosAsync();
        Task<IEnumerable<RacaEntity>> ObterAtivosAsync();
        Task<IEnumerable<RacaEntity>> ObterInativosAsync();
        Task<RacaEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<RacaEntity>> ObterPorEspecieAsync(int idEspecie);
        Task<RacaEntity?> AdicionarAsync(RacaEntity entity);
        Task<RacaEntity?> EditarAsync(int id, RacaEntity entity);
        Task<RacaEntity?> ReativarAsync(int id);
        Task<RacaEntity?> InativarAsync(int id);
    }
}
