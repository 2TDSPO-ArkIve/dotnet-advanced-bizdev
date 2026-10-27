using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.UseCases
{
    public class DoencaUseCase : IDoencaUseCase
    {
        private readonly IDoencaRepository _doencaRepository;
        private readonly ICategoriaDoencaRepository _categoriaDoencaRepository;
        private readonly ILogger<DoencaUseCase> _logger;

        public DoencaUseCase(IDoencaRepository doencaRepository, ICategoriaDoencaRepository categoriaDoencaRepository, ILogger<DoencaUseCase> logger)
        {
            _doencaRepository = doencaRepository;
            _categoriaDoencaRepository = categoriaDoencaRepository;
            _logger = logger;
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>> ObterTodasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo doenças (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _doencaRepository.ObterTodosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Nenhuma doença encontrada", 204);

                resultado.Links = Pagination.Links("/api/doencas", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                resultado.Links.Add(new("/api/doencas", "create_doenca", "POST"));
                return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças");
                return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Ocorreu um erro ao obter as doenças");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>> ObterAtivasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo doenças ativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _doencaRepository.ObterAtivosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Nenhuma doença ativa encontrada", 204);

                resultado.Links = Pagination.Links("/api/doencas/ativos", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças ativas");
                return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Ocorreu um erro ao obter as doenças ativas");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>> ObterInativasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo doenças inativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _doencaRepository.ObterInativosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Nenhuma doença inativa encontrada", 204);

                resultado.Links = Pagination.Links("/api/doencas/inativos", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças inativas");
                return OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Ocorreu um erro ao obter as doenças inativas");
            }
        }

        public async Task<OperationResult<GetDoencaDto?>> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo doença {Id}", id);

            try
            {
                var doenca = await _doencaRepository.ObterPorIdAsync(id);

                if (doenca is null)
                {
                    _logger.LogWarning("Doença {Id} não encontrada", id);
                    return OperationResult<GetDoencaDto?>.Failure("Doença não encontrada", 404);
                }

                return OperationResult<GetDoencaDto?>.Success(ItemDto(doenca, naLista: false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doença {Id}", id);
                return OperationResult<GetDoencaDto?>.Failure("Ocorreu um erro ao obter a doença");
            }
        }

        public async Task<OperationResult<IEnumerable<GetDoencaDto>>> ObterPorNomeAsync(string nome)
        {
            _logger.LogInformation("Obtendo doenças por nome {Nome}", nome);

            try
            {
                var resultado = await _doencaRepository.ObterPorNomeAsync(nome);

                if (!resultado.Any())
                    return OperationResult<IEnumerable<GetDoencaDto>>.Failure("Nenhuma doença encontrada com esse nome", 204);

                return OperationResult<IEnumerable<GetDoencaDto>>.Success(resultado.Select(x => ItemDto(x)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças por nome {Nome}", nome);
                return OperationResult<IEnumerable<GetDoencaDto>>.Failure("Ocorreu um erro ao obter as doenças por nome");
            }
        }

        public async Task<OperationResult<IEnumerable<GetDoencaDto>>> ObterPorCategoriaAsync(int idCategoria)
        {
            _logger.LogInformation("Obtendo doenças da categoria {IdCategoria}", idCategoria);

            try
            {
                var resultado = await _doencaRepository.ObterPorCategoriaAsync(idCategoria);

                if (!resultado.Any())
                    return OperationResult<IEnumerable<GetDoencaDto>>.Failure("Nenhuma doença encontrada para esta categoria", 204);

                return OperationResult<IEnumerable<GetDoencaDto>>.Success(resultado.Select(x => ItemDto(x)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças da categoria {IdCategoria}", idCategoria);
                return OperationResult<IEnumerable<GetDoencaDto>>.Failure("Ocorreu um erro ao obter as doenças da categoria");
            }
        }

        public async Task<OperationResult<DoencaEntity?>> AdicionarAsync(DoencaRequestDto dto)
        {
            _logger.LogInformation("Adicionando doença {Nome}", dto.Nome);

            try
            {
                await ValidarCategoriaAsync(dto.IdCategoria);

                var doenca = await _doencaRepository.AdicionarAsync(dto.ToDoencaEntity());
                return OperationResult<DoencaEntity?>.Success(doenca, 201);
            }
            catch (CategoriaNaoEncontradaException ex)
            {
                return OperationResult<DoencaEntity?>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar doença {Nome}", dto.Nome);
                return OperationResult<DoencaEntity?>.Failure("Ocorreu um erro ao adicionar a doença");
            }
        }

        public async Task<OperationResult<DoencaEntity?>> EditarAsync(int id, DoencaRequestDto dto)
        {
            _logger.LogInformation("Editando doença {Id}", id);

            try
            {
                await ValidarCategoriaAsync(dto.IdCategoria);

                var doenca = await _doencaRepository.EditarAsync(id, dto.ToDoencaEntity());

                if (doenca is null)
                {
                    _logger.LogWarning("Doença {Id} não encontrada", id);
                    return OperationResult<DoencaEntity?>.Failure("Doença não encontrada", 404);
                }

                return OperationResult<DoencaEntity?>.Success(doenca);
            }
            catch (CategoriaNaoEncontradaException ex)
            {
                return OperationResult<DoencaEntity?>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar doença {Id}", id);
                return OperationResult<DoencaEntity?>.Failure("Ocorreu um erro ao editar a doença");
            }
        }

        public async Task<OperationResult<DoencaEntity?>> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando doença {Id}", id);

            try
            {
                var doenca = await _doencaRepository.ReativarAsync(id);

                if (doenca is null)
                {
                    _logger.LogWarning("Doença {Id} não encontrada", id);
                    return OperationResult<DoencaEntity?>.Failure("Doença não encontrada", 404);
                }

                return OperationResult<DoencaEntity?>.Success(doenca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar doença {Id}", id);
                return OperationResult<DoencaEntity?>.Failure("Ocorreu um erro ao reativar a doença");
            }
        }

        public async Task<OperationResult<DoencaEntity?>> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando doença {Id}", id);

            try
            {
                var doenca = await _doencaRepository.InativarAsync(id);

                if (doenca is null)
                {
                    _logger.LogWarning("Doença {Id} não encontrada", id);
                    return OperationResult<DoencaEntity?>.Failure("Doença não encontrada", 404);
                }

                return OperationResult<DoencaEntity?>.Success(doenca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar doença {Id}", id);
                return OperationResult<DoencaEntity?>.Failure("Ocorreu um erro ao inativar a doença");
            }
        }

        /// <summary>
        /// Regra de negócio: se uma categoria for informada, ela precisa existir e estar ativa.
        /// Categoria é opcional (IdCategoria é nullable), então null não gera erro.
        /// </summary>
        private async Task ValidarCategoriaAsync(int? idCategoria)
        {
            if (idCategoria is null)
                return;

            var categoria = await _categoriaDoencaRepository.ObterPorIdAsync(idCategoria.Value);

            if (categoria is null || categoria.StAtivo != "S")
            {
                _logger.LogWarning("Categoria {IdCategoria} inválida ou inativa", idCategoria.Value);
                throw new CategoriaNaoEncontradaException(idCategoria.Value);
            }
        }

        /// <summary>
        /// Converte a entidade no DTO de retorno com links HATEOAS de item (get_ na lista, self no GetById).
        /// </summary>
        private static GetDoencaDto ItemDto(DoencaEntity x, bool naLista = true)
            => x.ToDoencaDto(Pagination.ItemLinks("/api/doencas", x.Id, "doenca", ativo: x.StAtivo == "S", naLista: naLista));
    }
}
