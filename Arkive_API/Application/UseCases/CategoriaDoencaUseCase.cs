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

        public async Task<OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>> ObterTodasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo todas as categorias de doença (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _categoriaDoencaRepository.ObterTodosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Nenhuma categoria de doença encontrada", 204);

                resultado.Links = Pagination.Links("/api/categorias-doenca", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                resultado.Links.Add(new("/api/categorias-doenca", "create_categoria_doenca", "POST"));
                return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as categorias de doença");
                return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Ocorreu um erro ao obter as categorias de doença");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>> ObterAtivasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo categorias de doença ativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _categoriaDoencaRepository.ObterAtivosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Nenhuma categoria de doença ativa encontrada", 204);

                resultado.Links = Pagination.Links("/api/categorias-doenca/ativos", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias de doença ativas");
                return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Ocorreu um erro ao obter as categorias de doença ativas");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>> ObterInativasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo categorias de doença inativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _categoriaDoencaRepository.ObterInativosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Nenhuma categoria de doença inativa encontrada", 204);

                resultado.Links = Pagination.Links("/api/categorias-doenca/inativos", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias de doença inativas");
                return OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Ocorreu um erro ao obter as categorias de doença inativas");
            }
        }

        public async Task<OperationResult<GetCategoriaDoencaDto?>> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaRepository.ObterPorIdAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return OperationResult<GetCategoriaDoencaDto?>.Failure("Categoria de doença não encontrada", 404);
                }

                return OperationResult<GetCategoriaDoencaDto?>.Success(ItemDto(categoria, naLista: false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria de doença {Id}", id);
                return OperationResult<GetCategoriaDoencaDto?>.Failure("Ocorreu um erro ao obter a categoria de doença");
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

        /// <summary>
        /// Converte a entidade no DTO de retorno com links HATEOAS de item (get_ na lista, self no GetById).
        /// </summary>
        private static GetCategoriaDoencaDto ItemDto(CategoriaDoencaEntity x, bool naLista = true)
            => x.ToCategoriaDoencaDto(Pagination.ItemLinks("/api/categorias-doenca", x.Id, "categoria_doenca", ativo: x.StAtivo == "S", naLista: naLista));
    }
}
