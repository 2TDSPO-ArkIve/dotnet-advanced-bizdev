using Arkive_Application.Dtos;
using Arkive_Application.Interfaces;
using Arkive_Domain.Interfaces;
using Arkive_Domain.Models;

namespace Arkive_Application.UseCases
{
    public class AuditoriaUseCase : IAuditoriaUseCase
    {
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly ILogger<AuditoriaUseCase> _logger;

        public AuditoriaUseCase(IAuditoriaRepository auditoriaRepository, ILogger<AuditoriaUseCase> logger)
        {
            _auditoriaRepository = auditoriaRepository;
            _logger = logger;
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>> ObterTodasAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo auditoria (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                var resultado = await _auditoriaRepository.ObterTodosAsync(s, t);

                if (!resultado.Data.Any())
                    return OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>.Failure("Nenhum registro de auditoria encontrado", 204);

                resultado.Links = Pagination.Links("/api/auditoria", s, t, resultado.TotalRegistros);
                return OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>.Success(Pagination.MapearItens(resultado, x => new GetAuditoriaDto
                {
                    Id = x.Id.ToString(),
                    Usuario = x.Usuario,
                    Metodo = x.Metodo,
                    Rota = x.Rota,
                    StatusCode = x.StatusCode,
                    Data = x.Data,
                    Requisicao = x.Requisicao,
                    Resposta = x.Resposta
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a auditoria");
                return OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>.Failure("Ocorreu um erro ao obter a auditoria");
            }
        }
    }
}
