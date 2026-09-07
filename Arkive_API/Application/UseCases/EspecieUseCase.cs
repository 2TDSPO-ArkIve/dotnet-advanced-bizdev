using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class EspecieUseCase : IEspecieUseCase
    {
        private readonly IEspecieRepository _especieRepository;
        private readonly ILogger<EspecieUseCase> _logger;

        public EspecieUseCase(IEspecieRepository especieRepository, ILogger<EspecieUseCase> logger)
        {
            _especieRepository = especieRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<EspecieEntity>> ObterTodasAsync()
        {
            _logger.LogInformation("Obtendo todas as espécies");

            try
            {
                return await _especieRepository.ObterTodosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as espécies");
                throw;
            }
        }

        public async Task<IEnumerable<EspecieEntity>> ObterAtivasAsync()
        {
            _logger.LogInformation("Obtendo espécies ativas");

            try
            {
                return await _especieRepository.ObterAtivosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter espécies ativas");
                throw;
            }
        }

        public async Task<IEnumerable<EspecieEntity>> ObterInativasAsync()
        {
            _logger.LogInformation("Obtendo espécies inativas");

            try
            {
                return await _especieRepository.ObterInativosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter espécies inativas");
                throw;
            }
        }

        public async Task<EspecieEntity?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo espécie {Id}", id);

            try
            {
                return await _especieRepository.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter espécie {Id}", id);
                throw;
            }
        }

        public async Task<EspecieEntity?> AdicionarAsync(EspecieRequestDto dto)
        {
            _logger.LogInformation("Adicionando espécie {Nome}", dto.Especie);

            try
            {
                return await _especieRepository.AdicionarAsync(dto.ToEspecieEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar espécie {Nome}", dto.Especie);
                throw;
            }
        }

        public async Task<EspecieEntity?> EditarAsync(int id, EspecieRequestDto dto)
        {
            _logger.LogInformation("Editando espécie {Id}", id);

            try
            {
                return await _especieRepository.EditarAsync(id, dto.ToEspecieEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar espécie {Id}", id);
                throw;
            }
        }

        public async Task<EspecieEntity?> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando espécie {Id}", id);

            try
            {
                return await _especieRepository.ReativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar espécie {Id}", id);
                throw;
            }
        }

        public async Task<EspecieEntity?> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando espécie {Id}", id);

            try
            {
                return await _especieRepository.InativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar espécie {Id}", id);
                throw;
            }
        }
    }
}
