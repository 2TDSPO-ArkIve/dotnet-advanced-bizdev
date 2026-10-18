using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;

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

        public async Task<OperationResult<IEnumerable<RacaEntity>>> ObterTodasAsync()
        {
            _logger.LogInformation("Obtendo todas as raças");

            try
            {
                var resultado = await _racaRepository.ObterTodosAsync();

                if (!resultado.Any())
                    return OperationResult<IEnumerable<RacaEntity>>.Failure("Nenhuma raça encontrada", 204);

                return OperationResult<IEnumerable<RacaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as raças");
                return OperationResult<IEnumerable<RacaEntity>>.Failure("Ocorreu um erro ao obter as raças");
            }
        }

        public async Task<OperationResult<IEnumerable<RacaEntity>>> ObterAtivasAsync()
        {
            _logger.LogInformation("Obtendo raças ativas");

            try
            {
                var resultado = await _racaRepository.ObterAtivosAsync();

                if (!resultado.Any())
                    return OperationResult<IEnumerable<RacaEntity>>.Failure("Nenhuma raça ativa encontrada", 204);

                return OperationResult<IEnumerable<RacaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raças ativas");
                return OperationResult<IEnumerable<RacaEntity>>.Failure("Ocorreu um erro ao obter as raças ativas");
            }
        }

        public async Task<OperationResult<IEnumerable<RacaEntity>>> ObterInativasAsync()
        {
            _logger.LogInformation("Obtendo raças inativas");

            try
            {
                var resultado = await _racaRepository.ObterInativosAsync();

                if (!resultado.Any())
                    return OperationResult<IEnumerable<RacaEntity>>.Failure("Nenhuma raça inativa encontrada", 204);

                return OperationResult<IEnumerable<RacaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raças inativas");
                return OperationResult<IEnumerable<RacaEntity>>.Failure("Ocorreu um erro ao obter as raças inativas");
            }
        }

        public async Task<OperationResult<GetRacaDto?>> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo raça {Id}", id);

            try
            {
                var raca = await _racaRepository.ObterPorIdAsync(id);

                if (raca is null)
                {
                    _logger.LogWarning("Raça {Id} não encontrada", id);
                    return OperationResult<GetRacaDto?>.Failure("Raça não encontrada", 404);
                }

                return OperationResult<GetRacaDto?>.Success(raca.ToRacaDto(Pagination.ItemLinks("/api/racas", id, "raca")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raça {Id}", id);
                return OperationResult<GetRacaDto?>.Failure("Ocorreu um erro ao obter a raça");
            }
        }

        public async Task<OperationResult<IEnumerable<RacaEntity>>> ObterPorEspecieAsync(int idEspecie)
        {
            _logger.LogInformation("Obtendo raças da espécie {IdEspecie}", idEspecie);

            try
            {
                var resultado = await _racaRepository.ObterPorEspecieAsync(idEspecie);

                if (!resultado.Any())
                    return OperationResult<IEnumerable<RacaEntity>>.Failure("Nenhuma raça encontrada para esta espécie", 204);

                return OperationResult<IEnumerable<RacaEntity>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter raças da espécie {IdEspecie}", idEspecie);
                return OperationResult<IEnumerable<RacaEntity>>.Failure("Ocorreu um erro ao obter as raças da espécie");
            }
        }

        public async Task<OperationResult<RacaEntity?>> AdicionarAsync(RacaRequestDto dto)
        {
            _logger.LogInformation("Adicionando raça {Nome} para espécie {IdEspecie}", dto.Raca, dto.IdEspecie);

            try
            {
                await ValidarEspecieAsync(dto.IdEspecie);

                var raca = await _racaRepository.AdicionarAsync(dto.ToRacaEntity());
                return OperationResult<RacaEntity?>.Success(raca, 201);
            }
            catch (EspecieNaoEncontradaException ex)
            {
                return OperationResult<RacaEntity?>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar raça {Nome}", dto.Raca);
                return OperationResult<RacaEntity?>.Failure("Ocorreu um erro ao adicionar a raça");
            }
        }

        public async Task<OperationResult<RacaEntity?>> EditarAsync(int id, RacaRequestDto dto)
        {
            _logger.LogInformation("Editando raça {Id}", id);

            try
            {
                await ValidarEspecieAsync(dto.IdEspecie);

                var raca = await _racaRepository.EditarAsync(id, dto.ToRacaEntity());

                if (raca is null)
                {
                    _logger.LogWarning("Raça {Id} não encontrada", id);
                    return OperationResult<RacaEntity?>.Failure("Raça não encontrada", 404);
                }

                return OperationResult<RacaEntity?>.Success(raca);
            }
            catch (EspecieNaoEncontradaException ex)
            {
                return OperationResult<RacaEntity?>.Failure(ex.Message, 404);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar raça {Id}", id);
                return OperationResult<RacaEntity?>.Failure("Ocorreu um erro ao editar a raça");
            }
        }

        public async Task<OperationResult<RacaEntity?>> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando raça {Id}", id);

            try
            {
                var raca = await _racaRepository.ReativarAsync(id);

                if (raca is null)
                {
                    _logger.LogWarning("Raça {Id} não encontrada", id);
                    return OperationResult<RacaEntity?>.Failure("Raça não encontrada", 404);
                }

                return OperationResult<RacaEntity?>.Success(raca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar raça {Id}", id);
                return OperationResult<RacaEntity?>.Failure("Ocorreu um erro ao reativar a raça");
            }
        }

        public async Task<OperationResult<RacaEntity?>> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando raça {Id}", id);

            try
            {
                var raca = await _racaRepository.InativarAsync(id);

                if (raca is null)
                {
                    _logger.LogWarning("Raça {Id} não encontrada", id);
                    return OperationResult<RacaEntity?>.Failure("Raça não encontrada", 404);
                }

                return OperationResult<RacaEntity?>.Success(raca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar raça {Id}", id);
                return OperationResult<RacaEntity?>.Failure("Ocorreu um erro ao inativar a raça");
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
