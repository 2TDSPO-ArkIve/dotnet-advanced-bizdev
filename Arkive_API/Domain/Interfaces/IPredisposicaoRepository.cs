using Arkive_API.Domain.Entities;

namespace Arkive_API.Domain.Interfaces
{
    public interface IPredisposicaoRepository
    {
        Task<IEnumerable<PredisposicaoEntity>> ObterTodosAsync();
        Task<PredisposicaoEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<PredisposicaoEntity>> ObterPorEspecieAsync(int idEspecie);
        Task<IEnumerable<PredisposicaoEntity>> ObterPorRacaAsync(int idRaca);
        Task<IEnumerable<PredisposicaoEntity>> ObterPorDoencaAsync(int idDoenca);
        Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoEntity entity);
        Task<PredisposicaoEntity?> DeletarAsync(int id);
    }
}
