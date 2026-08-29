using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class RacaUseCase : IRacaUseCase
    {
        private readonly IRacaRepository _racaRepository;
        private readonly IEspecieRepository _especieRepository;

        public RacaUseCase(IRacaRepository racaRepository, IEspecieRepository especieRepository)
        {
            _racaRepository = racaRepository;
            _especieRepository = especieRepository;
        }

        public async Task<IEnumerable<RacaEntity>> ObterTodasAsync()
        {
            return await _racaRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<RacaEntity>> ObterAtivasAsync()
        {
            return await _racaRepository.ObterAtivosAsync();
        }

        public async Task<IEnumerable<RacaEntity>> ObterInativasAsync()
        {
            return await _racaRepository.ObterInativosAsync();
        }

        public async Task<RacaEntity?> ObterPorIdAsync(int id)
        {
            return await _racaRepository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<RacaEntity>> ObterPorEspecieAsync(int idEspecie)
        {
            return await _racaRepository.ObterPorEspecieAsync(idEspecie);
        }

        public async Task<RacaEntity?> AdicionarAsync(RacaRequestDto dto)
        {
            await ValidarEspecieAsync(dto.IdEspecie);

            return await _racaRepository.AdicionarAsync(dto.ToRacaEntity());
        }

        public async Task<RacaEntity?> EditarAsync(int id, RacaRequestDto dto)
        {
            await ValidarEspecieAsync(dto.IdEspecie);

            return await _racaRepository.EditarAsync(id, dto.ToRacaEntity());
        }

        public async Task<RacaEntity?> ReativarAsync(int id)
        {
            return await _racaRepository.ReativarAsync(id);
        }

        public async Task<RacaEntity?> InativarAsync(int id)
        {
            return await _racaRepository.InativarAsync(id);
        }

        /// <summary>
        /// Regra de negócio: a espécie referenciada precisa existir e estar ativa.
        /// Diferente da Doenca (categoria opcional), aqui IdEspecie é obrigatório,
        /// então a validação roda sempre, tanto no Add quanto no Edit.
        /// </summary>
        private async Task ValidarEspecieAsync(int idEspecie)
        {
            var especie = await _especieRepository.ObterPorIdAsync(idEspecie);

            if (especie is null || especie.StAtivo != "S")
                throw new EspecieNaoEncontradaException(idEspecie);
        }
    }
}
