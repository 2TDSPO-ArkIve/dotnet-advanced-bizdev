using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;

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

        public async Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterTodasAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _predisposicaoRepository.ObterTodosAsync(s, t);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Nenhuma predisposição encontrada", 204);

                resultado.Links = Pagination.Links("/api/predisposicoes", s, t, resultado.TotalRegistros);
                resultado.Links.Add(new("/api/predisposicoes", "create_predisposicao", "POST"));
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições");
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Ocorreu um erro ao obter as predisposições");
            }
        }

        public async Task<OperationResult<GetPredisposicaoDto?>> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo predisposição {Id}", id);

            try
            {
                var predisposicao = await _predisposicaoRepository.ObterPorIdAsync(id);

                if (predisposicao is null)
                {
                    _logger.LogWarning("Predisposição {Id} não encontrada", id);
                    return OperationResult<GetPredisposicaoDto?>.Failure("Predisposição não encontrada", 404);
                }

                return OperationResult<GetPredisposicaoDto?>.Success(ItemDto(predisposicao, naLista: false));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposição {Id}", id);
                return OperationResult<GetPredisposicaoDto?>.Failure("Ocorreu um erro ao obter a predisposição");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições da espécie {IdEspecie} (skip {Skip}, take {Take})", idEspecie, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _predisposicaoRepository.ObterPorEspecieAsync(idEspecie, s, t);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Nenhuma predisposição encontrada para esta espécie", 204);

                resultado.Links = Pagination.Links($"/api/predisposicoes/especie/{idEspecie}", s, t, resultado.TotalRegistros);
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições da espécie {IdEspecie}", idEspecie);
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Ocorreu um erro ao obter as predisposições da espécie");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições da raça {IdRaca} (skip {Skip}, take {Take})", idRaca, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _predisposicaoRepository.ObterPorRacaAsync(idRaca, s, t);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Nenhuma predisposição encontrada para esta raça", 204);

                resultado.Links = Pagination.Links($"/api/predisposicoes/raca/{idRaca}", s, t, resultado.TotalRegistros);
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições da raça {IdRaca}", idRaca);
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Ocorreu um erro ao obter as predisposições da raça");
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo predisposições da doença {IdDoenca} (skip {Skip}, take {Take})", idDoenca, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _predisposicaoRepository.ObterPorDoencaAsync(idDoenca, s, t);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Nenhuma predisposição encontrada para esta doença", 204);

                resultado.Links = Pagination.Links($"/api/predisposicoes/doenca/{idDoenca}", s, t, resultado.TotalRegistros);
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Success(Pagination.MapearItens(resultado, x => ItemDto(x)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter predisposições da doença {IdDoenca}", idDoenca);
                return OperationResult<PageResultModel<IEnumerable<GetPredisposicaoDto>>>.Failure("Ocorreu um erro ao obter as predisposições da doença");
            }
        }

        public async Task<OperationResult<PredisposicaoEntity?>> AdicionarAsync(PredisposicaoRequestDto dto)
        {
            _logger.LogInformation("Adicionando predisposição (espécie {IdEspecie}, raça {IdRaca}, doença {IdDoenca})", dto.IdEspecie, dto.IdRaca, dto.IdDoenca);

            try
            {
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

                var predisposicao = await _predisposicaoRepository.AdicionarAsync(dto.ToPredisposicaoEntity());
                return OperationResult<PredisposicaoEntity?>.Success(predisposicao, 201);
            }
            catch (Exception ex) when (ex is EspecieNaoEncontradaException or RacaNaoEncontradaException or DoencaNaoEncontradaException)
            {
                return OperationResult<PredisposicaoEntity?>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar predisposição");
                return OperationResult<PredisposicaoEntity?>.Failure("Ocorreu um erro ao adicionar a predisposição");
            }
        }

        public async Task<OperationResult<PredisposicaoEntity?>> DeletarAsync(int id)
        {
            _logger.LogInformation("Removendo predisposição {Id}", id);

            try
            {
                var predisposicao = await _predisposicaoRepository.DeletarAsync(id);

                if (predisposicao is null)
                {
                    _logger.LogWarning("Predisposição {Id} não encontrada", id);
                    return OperationResult<PredisposicaoEntity?>.Failure("Predisposição não encontrada", 404);
                }

                return OperationResult<PredisposicaoEntity?>.Success(predisposicao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover predisposição {Id}", id);
                return OperationResult<PredisposicaoEntity?>.Failure("Ocorreu um erro ao remover a predisposição");
            }
        }

        /// <summary>
        /// Converte a entidade no DTO de retorno com links HATEOAS de item (get_ na lista, self no GetById).
        /// </summary>
        private static GetPredisposicaoDto ItemDto(PredisposicaoEntity x, bool naLista = true)
            => x.ToPredisposicaoDto(Pagination.ItemLinks("/api/predisposicoes", x.Id, "predisposicao", editavel: false, naLista: naLista));
    }
}
