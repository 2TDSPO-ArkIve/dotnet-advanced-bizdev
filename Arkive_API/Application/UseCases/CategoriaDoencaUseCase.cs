using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class CategoriaDoencaUseCase : ICategoriaDoencaUseCase
    {
        private readonly ICategoriaDoencaRepository _categoriaDoencaRepository;
        private readonly ILogger<CategoriaDoencaUseCase> _logger;

        public CategoriaDoencaUseCase(ICategoriaDoencaRepository categoriaDoencaRepository, ILogger<CategoriaDoencaUseCase> logger)
        {
            _categoriaDoencaRepository = categoriaDoencaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterTodasAsync()
        {
            _logger.LogInformation("Obtendo todas as categorias de doença");

            try
            {
                return await _categoriaDoencaRepository.ObterTodosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as categorias de doença");
                throw;
            }
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterAtivasAsync()
        {
            _logger.LogInformation("Obtendo categorias de doença ativas");

            try
            {
                return await _categoriaDoencaRepository.ObterAtivosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias de doença ativas");
                throw;
            }
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterInativasAsync()
        {
            _logger.LogInformation("Obtendo categorias de doença inativas");

            try
            {
                return await _categoriaDoencaRepository.ObterInativosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias de doença inativas");
                throw;
            }
        }

        public async Task<CategoriaDoencaEntity?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo categoria de doença {Id}", id);

            try
            {
                return await _categoriaDoencaRepository.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria de doença {Id}", id);
                throw;
            }
        }

        public async Task<CategoriaDoencaEntity?> AdicionarAsync(CategoriaDoencaRequestDto dto)
        {
            _logger.LogInformation("Adicionando categoria de doença {Nome}", dto.Nome);

            try
            {
                return await _categoriaDoencaRepository.AdicionarAsync(dto.ToCategoriaDoencaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar categoria de doença {Nome}", dto.Nome);
                throw;
            }
        }

        public async Task<CategoriaDoencaEntity?> EditarAsync(int id, CategoriaDoencaRequestDto dto)
        {
            _logger.LogInformation("Editando categoria de doença {Id}", id);

            try
            {
                return await _categoriaDoencaRepository.EditarAsync(id, dto.ToCategoriaDoencaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar categoria de doença {Id}", id);
                throw;
            }
        }

        public async Task<CategoriaDoencaEntity?> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando categoria de doença {Id}", id);

            try
            {
                return await _categoriaDoencaRepository.ReativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar categoria de doença {Id}", id);
                throw;
            }
        }

        public async Task<CategoriaDoencaEntity?> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando categoria de doença {Id}", id);

            try
            {
                return await _categoriaDoencaRepository.InativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar categoria de doença {Id}", id);
                throw;
            }
        }
    }
}
