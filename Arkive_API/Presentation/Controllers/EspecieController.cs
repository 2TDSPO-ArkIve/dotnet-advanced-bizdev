using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/especies")]
    [ApiController]
    [EnableRateLimiting("leitura")]
    public class EspecieController : ControllerBase
    {
        private readonly IEspecieUseCase _especieUseCase;
        private readonly ILogger<EspecieController> _logger;

        public EspecieController(IEspecieUseCase especieUseCase, ILogger<EspecieController> logger)
        {
            _especieUseCase = especieUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as espécies",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo todas as espécies cadastradas, ativas e inativas.
            * **Status 204 (No Content):** Executado com sucesso, porém a base não possui espécies cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Este endpoint não filtra por status; use `/ativos` ou `/inativos` para isso.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(EspecieResponseListSample))]
        public async Task<IActionResult> GetAllEspecies()
        {
            _logger.LogInformation("Listando todas as espécies");

            try
            {
                var resultado = await _especieUseCase.ObterTodasAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ativos")]
        [SwaggerOperation(
            Summary = "Lista espécies ativas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as espécies com status ativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há espécies ativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Espécies inativas (excluídas logicamente) não aparecem neste retorno.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetEspeciesAtivas()
        {
            _logger.LogInformation("Listando espécies ativas");

            try
            {
                var resultado = await _especieUseCase.ObterAtivasAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("inativos")]
        [SwaggerOperation(
            Summary = "Lista espécies inativas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as espécies com status inativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há espécies inativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Espécies inativas são registros excluídos logicamente (soft delete), não removidos fisicamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetEspeciesInativas()
        {
            _logger.LogInformation("Listando espécies inativas");

            try
            {
                var resultado = await _especieUseCase.ObterInativasAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Busca espécie por ID",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a espécie correspondente ao ID informado.
            * **Status 404 (Not Found):** Nenhuma espécie foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * A busca por ID retorna a espécie independente do status (ativa ou inativa).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie retornada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(EspecieResponseSample))]
        public async Task<IActionResult> GetEspecieById(int id)
        {
            _logger.LogInformation("Buscando espécie {Id}", id);

            try
            {
                var especie = await _especieUseCase.ObterPorIdAsync(id);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Cria uma nova espécie",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** A espécie foi cadastrada com sucesso.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados (ex: nome já cadastrado).

            ## Observações:
            * O nome da espécie deve ser único no sistema.
            * A espécie é criada sempre com status ativo.
            """
        )]
        [SwaggerRequestExample(typeof(EspecieRequestDto), typeof(EspecieRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Espécie criada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a espécie", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(EspecieResponseSample))]
        public async Task<IActionResult> CreateEspecie(EspecieRequestDto model)
        {
            _logger.LogInformation("Criando espécie {Nome}", model.Especie);

            try
            {
                var especie = await _especieUseCase.AdicionarAsync(model);

                _logger.LogInformation("Espécie criada com sucesso: {Id}", especie?.Id ?? 0);
                return CreatedAtAction(nameof(GetEspecieById), new { id = especie?.Id ?? 0 }, especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Atualiza uma espécie",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A espécie foi atualizada com sucesso.
            * **Status 404 (Not Found):** Nenhuma espécie ativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados.

            ## Observações:
            * Somente espécies ativas podem ser atualizadas.
            * Espécies inativas devem ser reativadas antes de serem editadas.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie atualizada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a espécie", type: typeof(string))]
        public async Task<IActionResult> UpdateEspecie(int id, EspecieRequestDto model)
        {
            _logger.LogInformation("Atualizando espécie {Id}", id);

            try
            {
                var especie = await _especieUseCase.EditarAsync(id, model);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reativar/{id}")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Reativa uma espécie",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A espécie foi reativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma espécie inativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao reativar a espécie.

            ## Observações:
            * Somente espécies com status inativo podem ser reativadas.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie reativada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a espécie", type: typeof(string))]
        public async Task<IActionResult> ReactivateEspecie(int id)
        {
            _logger.LogInformation("Reativando espécie {Id}", id);

            try
            {
                var especie = await _especieUseCase.ReativarAsync(id);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Inativa uma espécie",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A espécie foi inativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma espécie ativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao inativar a espécie.

            ## Observações:
            * Esta operação realiza uma exclusão lógica (soft delete); o registro não é removido fisicamente do banco.
            * Uma espécie já inativa não pode ser inativada novamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie inativada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a espécie", type: typeof(string))]
        public async Task<IActionResult> DeleteEspecie(int id)
        {
            _logger.LogInformation("Inativando espécie {Id}", id);

            try
            {
                var especie = await _especieUseCase.InativarAsync(id);

                if (especie is null)
                {
                    _logger.LogWarning("Espécie {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(especie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }
    }
}
