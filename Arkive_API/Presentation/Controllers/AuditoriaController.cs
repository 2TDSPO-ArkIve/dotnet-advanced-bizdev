using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/auditoria")]
    [ApiController]
    [Authorize(Roles = Roles.SysAdmin)]
    [SwaggerResponse(statusCode: 401, description: "Token ausente, inválido ou expirado")]
    [SwaggerResponse(statusCode: 403, description: "Perfil sem permissão (exige SYSADMIN)")]
    [EnableRateLimiting("leitura")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaUseCase _auditoriaUseCase;
        private readonly ILogger<AuditoriaController> _logger;

        public AuditoriaController(IAuditoriaUseCase auditoriaUseCase, ILogger<AuditoriaController> logger)
        {
            _auditoriaUseCase = auditoriaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista a auditoria de escritas e logins",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a página de registros de auditoria, do mais recente para o mais antigo.
            * **Status 204 (No Content):** Nenhum registro de auditoria gravado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o MongoDB).

            ## Observações:
            * Os registros são gravados no MongoDB para toda requisição que não seja GET (POST, PUT, DELETE e login).
            * Acesso restrito ao perfil SYSADMIN.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetAuditoriaDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum registro encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(AuditoriaResponseListSample))]
        public async Task<IActionResult> GetAllAuditoria(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Listando auditoria (skip {Skip}, take {Take})", skip, take);

            var resultado = await _auditoriaUseCase.ObterTodasAsync(skip, take);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }
    }
}
