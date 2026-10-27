using Arkive_Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Arkive_API.Presentation.Handlers
{
    /// <summary>
    /// Rede final para exceções que escaparam dos UseCases (que já devolvem OperationResult).
    /// Responde application/problem+json; exceções de negócio viram 404/400 com a própria mensagem,
    /// qualquer outra vira 500 com mensagem genérica (sem stack trace nem ex.Message).
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetails;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(IProblemDetailsService problemDetails, ILogger<GlobalExceptionHandler> logger)
        {
            _problemDetails = problemDetails;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (status, titulo, detalhe) = exception switch
            {
                NaoEncontradaException => (StatusCodes.Status404NotFound, "Recurso não encontrado", exception.Message),
                ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "Requisição inválida", exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno", "Ocorreu um erro inesperado ao processar a requisição.")
            };

            if (status == StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Exceção não tratada em {Metodo} {Rota}", httpContext.Request.Method, httpContext.Request.Path);
            else
                _logger.LogWarning("{Titulo} em {Metodo} {Rota}: {Mensagem}", titulo, httpContext.Request.Method, httpContext.Request.Path, exception.Message);

            httpContext.Response.StatusCode = status;
            return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails { Status = status, Title = titulo, Detail = detalhe }
            });
        }
    }
}
