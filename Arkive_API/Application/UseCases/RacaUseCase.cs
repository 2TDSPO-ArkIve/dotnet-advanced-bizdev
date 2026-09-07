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
        private readonly ILogger<RacaUseCase> _logger;

        public RacaUseCase(IRacaRepository racaRepository, IEspecieRepository especieRepository, ILogger<RacaUseCase> logger)
        {
            _racaRepository = racaRepository;
            _especieRepository = especieRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<RacaEntity>> ObterTodasAsync()
        {
            _logger.LogInformation("Obtendo todas as raças");

            try
            {
                return await _racaRepository.ObterTodosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as raças");
                throw;
            }
        }

        public async Task<IEnumerable<RacaEntity>> ObterAtivasAsync()
        {
            _logger.LogInformation("Obtendo raças ativas");

            try
            {
                return await _racaRepository.ObterAtivosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raças ativas");
                throw;
            }
        }

        public async Task<IEnumerable<RacaEntity>> ObterInativasAsync()
        {
            _logger.LogInformation("Obtendo raças inativas");

            try
            {
                return await _racaRepository.ObterInativosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raças inativas");
                throw;
            }
        }

        public async Task<RacaEntity?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo raça {Id}", id);

            try
            {
                return await _racaRepository.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raça {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<RacaEntity>> ObterPorEspecieAsync(int idEspecie)
        {
            _logger.LogInformation("Obtendo raças da espécie {IdEspecie}", idEspecie);

            try
            {
                return await _racaRepository.ObterPorEspecieAsync(idEspecie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raças da espécie {IdEspecie}", idEspecie);
                throw;
            }
        }

        public async Task<RacaEntity?> AdicionarAsync(RacaRequestDto dto)
        {
            _logger.LogInformation("Adicionando raça {Nome} para espécie {IdEspecie}", dto.Raca, dto.IdEspecie);

            await ValidarEspecieAsync(dto.IdEspecie);

            try
            {
                return await _racaRepository.AdicionarAsync(dto.ToRacaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar raça {Nome}", dto.Raca);
                throw;
            }
        }

        public async Task<RacaEntity?> EditarAsync(int id, RacaRequestDto dto)
        {
            _logger.LogInformation("Editando raça {Id}", id);

            await ValidarEspecieAsync(dto.IdEspecie);

            try
            {
                return await _racaRepository.EditarAsync(id, dto.ToRacaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar raça {Id}", id);
                throw;
            }
        }

        public async Task<RacaEntity?> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando raça {Id}", id);

            try
            {
                return await _racaRepository.ReativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar raça {Id}", id);
                throw;
            }
        }

        public async Task<RacaEntity?> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando raça {Id}", id);

            try
            {
                return await _racaRepository.InativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar raça {Id}", id);
                throw;
            }
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
            {
                _logger.LogWarning("Espécie {IdEspecie} inválida ou inativa", idEspecie);
                throw new EspecieNaoEncontradaException(idEspecie);
            }
        }
    }
}
