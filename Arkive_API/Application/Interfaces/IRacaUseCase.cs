using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Interfaces
{
    public interface IRacaUseCase
    {
        Task<IEnumerable<RacaEntity>> ObterTodasAsync();
        Task<IEnumerable<RacaEntity>> ObterAtivasAsync();
        Task<IEnumerable<RacaEntity>> ObterInativasAsync();
        Task<RacaEntity?> ObterPorIdAsync(int id);
        Task<IEnumerable<RacaEntity>> ObterPorEspecieAsync(int idEspecie);

        /// <exception cref="Arkive_API.Application.Exceptions.EspecieNaoEncontradaException">
        /// Lançada quando dto.IdEspecie não existe ou está inativo.
        /// </exception>
        Task<RacaEntity?> AdicionarAsync(RacaRequestDto dto);

        /// <exception cref="Arkive_API.Application.Exceptions.EspecieNaoEncontradaException">
        /// Lançada quando dto.IdEspecie não existe ou está inativo.
        /// </exception>
        Task<RacaEntity?> EditarAsync(int id, RacaRequestDto dto);

        Task<RacaEntity?> ReativarAsync(int id);
        Task<RacaEntity?> InativarAsync(int id);
    }
}
