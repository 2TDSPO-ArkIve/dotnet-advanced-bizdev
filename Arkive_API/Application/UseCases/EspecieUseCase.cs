using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;

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

        public async Task<OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>> ObterTodasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo todas as espécies (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _especieRepository.ObterTodosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Failure("Nenhuma espécie encontrada", 204);

                resultado.Links = Pagination.Links("/api/especies", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                resultado.Links.Add(new("/api/especies", "create_especie", "POST"));
                return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as espécies");
                return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Failure("Ocorreu um erro ao obter as espécies");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>> ObterAtivasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo espécies ativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _especieRepository.ObterAtivosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Failure("Nenhuma espécie ativa encontrada", 204);

                resultado.Links = Pagination.Links("/api/especies/ativos", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter espécies ativas");
                return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Failure("Ocorreu um erro ao obter as espécies ativas");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>> ObterInativasAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Obtendo espécies inativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _especieRepository.ObterInativosAsync(s, t, ordenarPor, direcao);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Failure("Nenhuma espécie inativa encontrada", 204);

                resultado.Links = Pagination.Links("/api/especies/inativos", s, t, resultado.TotalRegistros, ordenarPor, direcao);
                return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter espécies inativas");
                return OperationResult<PageResultModel<IEnumerable<GetEspecieDto>>>.Failure("Ocorreu um erro ao obter as espécies inativas");
            }
        }

        public async Task<OperationResult<GetEspecieDto?>> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo espécie {Id}", id);

            try
            {
                var especie = await _especieRepository.ObterPorIdAsync(id);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return OperationResult<GetEspecieDto?>.Failure("Espécie não encontrada", 404);
                }

                return OperationResult<GetEspecieDto?>.Success(ItemDto(especie, naLista: false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter espécie {Id}", id);
                return OperationResult<GetEspecieDto?>.Failure("Ocorreu um erro ao obter a espécie");
            }
        }

        public async Task<OperationResult<EspecieEntity?>> AdicionarAsync(EspecieRequestDto dto)
        {
            _logger.LogInformation("Adicionando espécie {Nome}", dto.Especie);

            try
            {
                var especie = await _especieRepository.AdicionarAsync(dto.ToEspecieEntity());
                return OperationResult<EspecieEntity?>.Success(especie, 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar espécie {Nome}", dto.Especie);
                return OperationResult<EspecieEntity?>.Failure("Ocorreu um erro ao adicionar a espécie");
            }
        }

        public async Task<OperationResult<EspecieEntity?>> EditarAsync(int id, EspecieRequestDto dto)
        {
            _logger.LogInformation("Editando espécie {Id}", id);

            try
            {
                var especie = await _especieRepository.EditarAsync(id, dto.ToEspecieEntity());

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return OperationResult<EspecieEntity?>.Failure("Espécie não encontrada", 404);
                }

                return OperationResult<EspecieEntity?>.Success(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar espécie {Id}", id);
                return OperationResult<EspecieEntity?>.Failure("Ocorreu um erro ao editar a espécie");
            }
        }

        public async Task<OperationResult<EspecieEntity?>> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando espécie {Id}", id);

            try
            {
                var especie = await _especieRepository.ReativarAsync(id);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return OperationResult<EspecieEntity?>.Failure("Espécie não encontrada", 404);
                }

                return OperationResult<EspecieEntity?>.Success(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar espécie {Id}", id);
                return OperationResult<EspecieEntity?>.Failure("Ocorreu um erro ao reativar a espécie");
            }
        }

        public async Task<OperationResult<EspecieEntity?>> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando espécie {Id}", id);

            try
            {
                var especie = await _especieRepository.InativarAsync(id);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return OperationResult<EspecieEntity?>.Failure("Espécie não encontrada", 404);
                }

                return OperationResult<EspecieEntity?>.Success(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar espécie {Id}", id);
                return OperationResult<EspecieEntity?>.Failure("Ocorreu um erro ao inativar a espécie");
            }
        }

        /// <summary>
        /// Converte a entidade no DTO de retorno com links HATEOAS de item (get_ na lista, self no GetById).
        /// </summary>
        private static GetEspecieDto ItemDto(EspecieEntity x, bool naLista = true)
            => x.ToEspecieDto(Pagination.ItemLinks("/api/especies", x.Id, "especie", ativo: x.StAtivo == "S", naLista: naLista));
    }
}
