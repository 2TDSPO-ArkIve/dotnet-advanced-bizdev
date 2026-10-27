using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Arkive_API.Presentation.Filters
{
    /// <summary>
    /// Grava no MongoDB toda requisição que não seja GET (escritas e login).
    /// Falha na auditoria só gera aviso: nunca derruba a requisição.
    /// </summary>
    public class AuditoriaFilter : IAsyncActionFilter
    {
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly ILogger<AuditoriaFilter> _logger;

        public AuditoriaFilter(IAuditoriaRepository auditoriaRepository, ILogger<AuditoriaFilter> logger)
        {
            _auditoriaRepository = auditoriaRepository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executado = await next();

            var request = context.HttpContext.Request;
            if (HttpMethods.IsGet(request.Method)) return;

            try
            {
                // No login ainda não há claim; o usuário é o login informado (a senha nunca é lida)
                var usuario = context.HttpContext.User.Identity?.Name
                    ?? context.ActionArguments.Values.OfType<PostAuthDto>().FirstOrDefault()?.Login
                    ?? "anonimo";
                var status = (executado.Result as IStatusCodeActionResult)?.StatusCode
                    ?? context.HttpContext.Response.StatusCode;

                await _auditoriaRepository.AdicionarAsync(new AuditoriaEntity
                {
                    Usuario = usuario,
                    Metodo = request.Method,
                    Rota = request.Path,
                    StatusCode = status,
                    Data = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao gravar auditoria de {Metodo} {Rota}", request.Method, request.Path);
            }
        }
    }
}
