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
        private readonly ILogger<PredisposicaoUseCase> _logger;

        public PredisposicaoUseCase(
            IPredisposicaoRepository predisposicaoRepository,
            IEspecieRepository especieRepository,
            IRacaRepository racaRepository,
            IDoencaRepository doencaRepository,
            ILogger<PredisposicaoUseCase> logger)
        {
            _predisposicaoRepository = predisposicaoRepository;
            _especieRepository = especieRepository;
            _racaRepository = racaRepository;
            _doencaRepository = doencaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterTodasAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _predisposicaoRepository.ObterTodosAsync(s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições");
                throw;
            }
        }

        public async Task<PredisposicaoEntity?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo predisposição {Id}", id);

            try
            {
                return await _predisposicaoRepository.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposição {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições da espécie {IdEspecie} (skip {Skip}, take {Take})", idEspecie, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _predisposicaoRepository.ObterPorEspecieAsync(idEspecie, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições da espécie {IdEspecie}", idEspecie);
                throw;
            }
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições da raça {IdRaca} (skip {Skip}, take {Take})", idRaca, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _predisposicaoRepository.ObterPorRacaAsync(idRaca, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições da raça {IdRaca}", idRaca);
                throw;
            }
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições da doença {IdDoenca} (skip {Skip}, take {Take})", idDoenca, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _predisposicaoRepository.ObterPorDoencaAsync(idDoenca, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições da doença {IdDoenca}", idDoenca);
                throw;
            }
        }

        public async Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoRequestDto dto)
        {
            _logger.LogInformation("Adicionando predisposição (espécie {IdEspecie}, raça {IdRaca}, doença {IdDoenca})", dto.IdEspecie, dto.IdRaca, dto.IdDoenca);

            var especie = await _especieRepository.ObterPorIdAsync(dto.IdEspecie);
            if (especie is null || especie.StAtivo != "S")
            {
                _logger.LogWarning("Espécie {IdEspecie} inválida ou inativa", dto.IdEspecie);
                throw new EspecieNaoEncontradaException(dto.IdEspecie);
            }

            if (dto.IdRaca is not null)
            {
                var raca = await _racaRepository.ObterPorIdAsync(dto.IdRaca.Value);
                if (raca is null || raca.StAtivo != "S")
                {
                    _logger.LogWarning("Raça {IdRaca} inválida ou inativa", dto.IdRaca.Value);
                    throw new RacaNaoEncontradaException(dto.IdRaca.Value);
                }
            }

            var doenca = await _doencaRepository.ObterPorIdAsync(dto.IdDoenca);
            if (doenca is null || doenca.StAtivo != "S")
            {
                _logger.LogWarning("Doença {IdDoenca} inválida ou inativa", dto.IdDoenca);
                throw new DoencaNaoEncontradaException(dto.IdDoenca);
            }

            try
            {
                return await _predisposicaoRepository.AdicionarAsync(dto.ToPredisposicaoEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar predisposição");
                throw;
            }
        }

        public async Task<PredisposicaoEntity?> DeletarAsync(int id)
        {
            _logger.LogInformation("Removendo predisposição {Id}", id);

            try
            {
                return await _predisposicaoRepository.DeletarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover predisposição {Id}", id);
                throw;
            }
        }
    }
}
