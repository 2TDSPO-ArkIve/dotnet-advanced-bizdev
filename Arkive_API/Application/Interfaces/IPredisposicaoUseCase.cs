using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface IPredisposicaoUseCase
    {
        Task<IEnumerable<PredisposicaoEntity>> ObterTodasAsync(int skip = 0, int take = 50);
        Task<PredisposicaoEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<PredisposicaoEntity>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50);
        Task<IEnumerable<PredisposicaoEntity>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50);
        Task<IEnumerable<PredisposicaoEntity>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50);

        /// <exception cref="Arkive_API.Application.Exceptions.EspecieNaoEncontradaException" />
        /// <exception cref="Arkive_API.Application.Exceptions.RacaNaoEncontradaException" />
        /// <exception cref="Arkive_API.Application.Exceptions.DoencaNaoEncontradaException" />
        Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoRequestDto dto);

        // Sem EditarAsync — conforme PRD §16.3.5, o PUT foi substituído por remover e recriar o vínculo.

        Task<PredisposicaoEntity?> DeletarAsync(int id);
    }
}
