using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class PredisposicaoUseCase : IPredisposicaoUseCase
    {
        private readonly IPredisposicaoRepository _predisposicaoRepository;
        private readonly IEspecieRepository _especieRepository;
        private readonly IRacaRepository _racaRepository;
        private readonly IDoencaRepository _doencaRepository;

        public PredisposicaoUseCase(
            IPredisposicaoRepository predisposicaoRepository,
            IEspecieRepository especieRepository,
            IRacaRepository racaRepository,
            IDoencaRepository doencaRepository)
        {
            _predisposicaoRepository = predisposicaoRepository;
            _especieRepository = especieRepository;
            _racaRepository = racaRepository;
            _doencaRepository = doencaRepository;
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterTodasAsync()
        {
            return await _predisposicaoRepository.ObterTodosAsync();
        }

        public async Task<PredisposicaoEntity?> ObterPorIdAsync(int id)
        {
            return await _predisposicaoRepository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorEspecieAsync(int idEspecie)
        {
            return await _predisposicaoRepository.ObterPorEspecieAsync(idEspecie);
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorRacaAsync(int idRaca)
        {
            return await _predisposicaoRepository.ObterPorRacaAsync(idRaca);
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorDoencaAsync(int idDoenca)
        {
            return await _predisposicaoRepository.ObterPorDoencaAsync(idDoenca);
        }

        public async Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoRequestDto dto)
        {
            var especie = await _especieRepository.ObterPorIdAsync(dto.IdEspecie);
            if (especie is null || especie.StAtivo != "S")
                throw new EspecieNaoEncontradaException(dto.IdEspecie);

            if (dto.IdRaca is not null)
            {
                var raca = await _racaRepository.ObterPorIdAsync(dto.IdRaca.Value);
                if (raca is null || raca.StAtivo != "S")
                    throw new RacaNaoEncontradaException(dto.IdRaca.Value);
            }

            var doenca = await _doencaRepository.ObterPorIdAsync(dto.IdDoenca);
            if (doenca is null || doenca.StAtivo != "S")
                throw new DoencaNaoEncontradaException(dto.IdDoenca);

            return await _predisposicaoRepository.AdicionarAsync(dto.ToPredisposicaoEntity());
        }

        public async Task<PredisposicaoEntity?> DeletarAsync(int id)
        {
            return await _predisposicaoRepository.DeletarAsync(id);
        }
    }
}
