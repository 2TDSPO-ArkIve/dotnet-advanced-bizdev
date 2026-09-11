using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Domain.Interfaces
{
    public interface IPredisposicaoRepository
    {
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<PredisposicaoEntity?> ObterPorIdAsync(int id);
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50);
        Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoEntity entity);
        Task<PredisposicaoEntity?> DeletarAsync(int id);
    }
}
