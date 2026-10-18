using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableRateLimiting("escrita")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthUseCase authUseCase, ILogger<AuthController> logger)
        {
            _authUseCase = authUseCase;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Autentica um usuário e gera o token JWT",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Credenciais válidas; retorna o login e o token JWT (validade de 8 horas).
            * **Status 401 (Unauthorized):** Login inexistente, usuário inativo ou senha incorreta.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao consultar os dados.

            ## Observações:
            * Os usuários são cadastrados pela API Java (tabela TB_ARKIVE_USUARIO).
            * Copie o valor de `jwt` e cole no botão **Authorize** (cadeado) do Swagger para acessar os endpoints protegidos.
            """
        )]
        [SwaggerRequestExample(typeof(PostAuthDto), typeof(AuthRequestSample))]
        [SwaggerResponse(statusCode: 200, description: "Autenticado com sucesso", type: typeof(GetAuthDto))]
        [SwaggerResponse(statusCode: 401, description: "Credenciais inválidas", type: typeof(string))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao autenticar", type: typeof(string))]
        public async Task<IActionResult> Login(PostAuthDto model)
        {
            _logger.LogInformation("Requisição de login para {Login}", model.Login);

            var resultado = await _authUseCase.AutenticarAsync(model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }
    }
}
