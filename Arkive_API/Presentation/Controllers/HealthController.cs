using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/health2")]
    [ApiController]
    [AllowAnonymous]
    [DisableRateLimiting]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthService;

        public HealthController(HealthCheckService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet("live")]
        [SwaggerOperation(
            Summary = "Liveness — a API está no ar?",
            Description = """
            Executa apenas o check `self` (sem I/O externo). Use para o orquestrador
            decidir se reinicia o processo.

            * **Status 200 (OK):** processo saudável.
            * **Status 503 (Service Unavailable):** processo travado / não saudável.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "API no ar")]
        [SwaggerResponse(statusCode: 503, description: "API não saudável")]
        public async Task<IActionResult> Live(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(
                r => r.Tags.Contains("live"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, result);
        }

        [HttpGet("db")]
        [SwaggerOperation(
            Summary = "Readiness — o banco está on-line?",
            Description = """
            Executa um ping leve no Oracle. Use para o load balancer anexar/desanexar
            a instância do pool de tráfego.

            * **Status 200 (OK):** banco acessível, instância pronta para tráfego.
            * **Status 503 (Service Unavailable):** banco indisponível.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Banco on-line")]
        [SwaggerResponse(statusCode: 503, description: "Banco indisponível")]
        public async Task<IActionResult> Ready(CancellationToken ct)
        {
            var report = await _healthService.CheckHealthAsync(
                r => r.Tags.Contains("db"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, result);
        }
    }
}
