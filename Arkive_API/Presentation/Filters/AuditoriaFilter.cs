using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;

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

                // Só DTOs do body (ids de rota já estão em Rota). Login fica de fora: senha e JWT nunca vão para o Mongo
                var corpo = context.ActionArguments.Values
                    .Where(v => v is not null and not PostAuthDto && v is not string && !v.GetType().IsPrimitive)
                    .ToList();
                var resposta = (executado.Result as ObjectResult)?.Value;

                await _auditoriaRepository.AdicionarAsync(new AuditoriaEntity
                {
                    Usuario = usuario,
                    Metodo = request.Method,
                    Rota = request.Path,
                    StatusCode = status,
                    Data = DateTime.UtcNow,
                    Requisicao = corpo.Count == 0 ? null : Serializar(corpo.Count == 1 ? corpo[0] : corpo),
                    Resposta = resposta is null or GetAuthDto ? null : Serializar(resposta)
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao gravar auditoria de {Metodo} {Rota}", request.Method, request.Path);
            }
        }

        // ponytail: trunca em 4000 chars (JSON cortado no fim); sem teto o documento pode crescer sem limite
        private static string Serializar(object valor)
        {
            var json = JsonSerializer.Serialize(valor);
            return json.Length <= 4000 ? json : json[..4000];
        }
    }
}
