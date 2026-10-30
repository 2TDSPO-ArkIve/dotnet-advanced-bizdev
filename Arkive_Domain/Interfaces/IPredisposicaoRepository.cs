using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Domain.Interfaces
{
    public interface IPredisposicaoRepository
    {
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PredisposicaoEntity?> ObterPorIdAsync(int id);
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc");
        Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoEntity entity);
        Task<PredisposicaoEntity?> DeletarAsync(int id);
    }
}
