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
    [Route("api/categorias-doenca")]
    [ApiController]
    [EnableRateLimiting("leitura")]
    public class CategoriaDoencaController : ControllerBase
    {
        private readonly ICategoriaDoencaUseCase _categoriaDoencaUseCase;
        private readonly ILogger<CategoriaDoencaController> _logger;

        public CategoriaDoencaController(ICategoriaDoencaUseCase categoriaDoencaUseCase, ILogger<CategoriaDoencaController> logger)
        {
            _categoriaDoencaUseCase = categoriaDoencaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as categorias de doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo todas as categorias de doença cadastradas, ativas e inativas.
            * **Status 204 (No Content):** Executado com sucesso, porém a base não possui categorias cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Este endpoint não filtra por status; use `/ativos` ou `/inativos` para isso.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaDoencaResponseListSample))]
        public async Task<IActionResult> GetAllCategorias()
        {
            _logger.LogInformation("Listando todas as categorias de doença");

            try
            {
                var resultado = await _categoriaDoencaUseCase.ObterTodasAsync();

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
            Summary = "Lista categorias de doença ativas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as categorias com status ativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há categorias ativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Categorias inativas (excluídas logicamente) não aparecem neste retorno.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetCategoriasAtivas()
        {
            _logger.LogInformation("Listando categorias de doença ativas");

            try
            {
                var resultado = await _categoriaDoencaUseCase.ObterAtivasAsync();

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
            Summary = "Lista categorias de doença inativas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as categorias com status inativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há categorias inativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Categorias inativas são registros excluídos logicamente (soft delete), não removidos fisicamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetCategoriasInativas()
        {
            _logger.LogInformation("Listando categorias de doença inativas");

            try
            {
                var resultado = await _categoriaDoencaUseCase.ObterInativasAsync();

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
            Summary = "Busca categoria de doença por ID",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a categoria correspondente ao ID informado.
            * **Status 404 (Not Found):** Nenhuma categoria foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * A busca por ID retorna a categoria independente do status (ativa ou inativa).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria retornada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaDoencaResponseSample))]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            _logger.LogInformation("Buscando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaUseCase.ObterPorIdAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(categoria);
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
            Summary = "Cria uma nova categoria de doença",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** A categoria foi cadastrada com sucesso.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados (ex: nome já cadastrado).

            ## Observações:
            * O nome da categoria deve ser único no sistema.
            * A categoria é criada sempre com status ativo.
            """
        )]
        [SwaggerRequestExample(typeof(CategoriaDoencaRequestDto), typeof(CategoriaDoencaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Categoria criada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a categoria", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(CategoriaDoencaResponseSample))]
        public async Task<IActionResult> CreateCategoria(CategoriaDoencaRequestDto model)
        {
            _logger.LogInformation("Criando categoria de doença {Nome}", model.Nome);

            try
            {
                var categoria = await _categoriaDoencaUseCase.AdicionarAsync(model);

                _logger.LogInformation("Categoria de doença criada com sucesso: {Id}", categoria?.Id ?? 0);
                return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria?.Id ?? 0 }, categoria);
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
            Summary = "Atualiza uma categoria de doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A categoria foi atualizada com sucesso.
            * **Status 404 (Not Found):** Nenhuma categoria ativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados.

            ## Observações:
            * Somente categorias ativas podem ser atualizadas.
            * Categorias inativas devem ser reativadas antes de serem editadas.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria atualizada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a categoria", type: typeof(string))]
        public async Task<IActionResult> UpdateCategoria(int id, CategoriaDoencaRequestDto model)
        {
            _logger.LogInformation("Atualizando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaUseCase.EditarAsync(id, model);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(categoria);
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
            Summary = "Reativa uma categoria de doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A categoria foi reativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma categoria inativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao reativar a categoria.

            ## Observações:
            * Somente categorias com status inativo podem ser reativadas.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria reativada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a categoria", type: typeof(string))]
        public async Task<IActionResult> ReactivateCategoria(int id)
        {
            _logger.LogInformation("Reativando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaUseCase.ReativarAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(categoria);
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
            Summary = "Inativa uma categoria de doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A categoria foi inativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma categoria ativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao inativar a categoria.

            ## Observações:
            * Esta operação realiza uma exclusão lógica (soft delete); o registro não é removido fisicamente do banco.
            * Uma categoria já inativa não pode ser inativada novamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria inativada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a categoria", type: typeof(string))]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            _logger.LogInformation("Inativando categoria de doença {Id}", id);

            try
            {
                var categoria = await _categoriaDoencaUseCase.InativarAsync(id);

                if (categoria is null)
                {
                    _logger.LogWarning("Categoria de doença {Id} não encontrada", id);
                    return NotFound();
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
                return BadRequest(ex.Message);
            }
        }
    }
}
