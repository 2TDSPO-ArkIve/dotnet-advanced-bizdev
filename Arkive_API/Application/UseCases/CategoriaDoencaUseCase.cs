using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;

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

        public async Task<OperationResult<IEnumerable<CategoriaDoencaEntity>>> ObterTodasAsync()
        {
            _logger.LogInformation("Obtendo todas as categorias de doença");

            try
            {
                var resultado = await _categoriaDoencaRepository.ObterTodosAsync();

                if (!resultado.Any())
                    return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Failure("Nenhuma categoria de doença encontrada", 204);

                return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as categorias de doença");
                return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Failure("Ocorreu um erro ao obter as categorias de doença");
            }
        }

        public async Task<OperationResult<IEnumerable<CategoriaDoencaEntity>>> ObterAtivasAsync()
        {
            _logger.LogInformation("Obtendo categorias de doença ativas");

            try
            {
                var resultado = await _categoriaDoencaRepository.ObterAtivosAsync();

                if (!resultado.Any())
                    return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Failure("Nenhuma categoria de doença ativa encontrada", 204);

                return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias de doença ativas");
                return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Failure("Ocorreu um erro ao obter as categorias de doença ativas");
            }
        }

        public async Task<OperationResult<IEnumerable<CategoriaDoencaEntity>>> ObterInativasAsync()
        {
            _logger.LogInformation("Obtendo categorias de doença inativas");

            try
            {
                var resultado = await _categoriaDoencaRepository.ObterInativosAsync();

                if (!resultado.Any())
                    return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Failure("Nenhuma categoria de doença inativa encontrada", 204);

                return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias de doença inativas");
                return OperationResult<IEnumerable<CategoriaDoencaEntity>>.Failure("Ocorreu um erro ao obter as categorias de doença inativas");
            }
        }

        public async Task<OperationResult<CategoriaDoencaEntity?>> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaRepository.ObterPorIdAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return OperationResult<CategoriaDoencaEntity?>.Failure("Categoria de doença não encontrada", 404);
                }

                return OperationResult<CategoriaDoencaEntity?>.Success(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria de doença {Id}", id);
                return OperationResult<CategoriaDoencaEntity?>.Failure("Ocorreu um erro ao obter a categoria de doença");
            }
        }

        public async Task<OperationResult<CategoriaDoencaEntity?>> AdicionarAsync(CategoriaDoencaRequestDto dto)
        {
            _logger.LogInformation("Adicionando categoria de doença {Nome}", dto.Nome);

            try
            {
                var categoria = await _categoriaDoencaRepository.AdicionarAsync(dto.ToCategoriaDoencaEntity());
                return OperationResult<CategoriaDoencaEntity?>.Success(categoria, 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar categoria de doença {Nome}", dto.Nome);
                return OperationResult<CategoriaDoencaEntity?>.Failure("Ocorreu um erro ao adicionar a categoria de doença");
            }
        }

        public async Task<OperationResult<CategoriaDoencaEntity?>> EditarAsync(int id, CategoriaDoencaRequestDto dto)
        {
            _logger.LogInformation("Editando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaRepository.EditarAsync(id, dto.ToCategoriaDoencaEntity());

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return OperationResult<CategoriaDoencaEntity?>.Failure("Categoria de doença não encontrada", 404);
                }

                return OperationResult<CategoriaDoencaEntity?>.Success(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar categoria de doença {Id}", id);
                return OperationResult<CategoriaDoencaEntity?>.Failure("Ocorreu um erro ao editar a categoria de doença");
            }
        }

        public async Task<OperationResult<CategoriaDoencaEntity?>> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaRepository.ReativarAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return OperationResult<CategoriaDoencaEntity?>.Failure("Categoria de doença não encontrada", 404);
                }

                return OperationResult<CategoriaDoencaEntity?>.Success(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar categoria de doença {Id}", id);
                return OperationResult<CategoriaDoencaEntity?>.Failure("Ocorreu um erro ao reativar a categoria de doença");
            }
        }

        public async Task<OperationResult<CategoriaDoencaEntity?>> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaRepository.InativarAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return OperationResult<CategoriaDoencaEntity?>.Failure("Categoria de doença não encontrada", 404);
                }

                return OperationResult<CategoriaDoencaEntity?>.Success(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar categoria de doença {Id}", id);
                return OperationResult<CategoriaDoencaEntity?>.Failure("Ocorreu um erro ao inativar a categoria de doença");
            }
        }
    }
}
