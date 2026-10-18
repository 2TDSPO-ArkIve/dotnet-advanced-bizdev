using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/doencas")]
    [ApiController]
    [Authorize]
    [SwaggerResponse(statusCode: 401, description: "Token ausente, inválido ou expirado")]
    [EnableRateLimiting("leitura")]
    public class DoencaController : ControllerBase
    {
        private readonly IDoencaUseCase _doencaUseCase;
        private readonly ILogger<DoencaController> _logger;

        public DoencaController(IDoencaUseCase doencaUseCase, ILogger<DoencaController> logger)
        {
            _doencaUseCase = doencaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as doenças",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo todas as doenças cadastradas, ativas e inativas.
            * **Status 204 (No Content):** Executado com sucesso, porém a base não possui doenças cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Categoria**), quando informada.
            * Este endpoint não filtra por status; use `/ativos` ou `/inativos` para isso.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<DoencaEntity>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DoencaResponseListSample))]
        public async Task<IActionResult> GetAllDoencas(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Listando doenças (skip {Skip}, take {Take})", skip, take);

            var resultado = await _doencaUseCase.ObterTodasAsync(skip, take);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpGet("ativos")]
        [SwaggerOperation(
            Summary = "Lista doenças ativas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as doenças com status ativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há doenças ativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Categoria**), quando informada.
            * Doenças inativas (excluídas logicamente) não aparecem neste retorno.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<DoencaEntity>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencasAtivas(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Listando doenças ativas (skip {Skip}, take {Take})", skip, take);

            var resultado = await _doencaUseCase.ObterAtivasAsync(skip, take);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpGet("inativos")]
        [SwaggerOperation(
            Summary = "Lista doenças inativas",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as doenças com status inativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há doenças inativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Categoria**), quando informada.
            * Doenças inativas são registros excluídos logicamente (soft delete), não removidos fisicamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<DoencaEntity>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencasInativas(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Listando doenças inativas (skip {Skip}, take {Take})", skip, take);

            var resultado = await _doencaUseCase.ObterInativasAsync(skip, take);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Busca doença por ID",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a doença correspondente ao ID informado.
            * **Status 404 (Not Found):** Nenhuma doença foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Categoria**), quando informada.
            * A busca por ID retorna a doença independente do status (ativa ou inativa).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença retornada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DoencaResponseSample))]
        public async Task<IActionResult> GetDoencaById(int id)
        {
            _logger.LogInformation("Buscando doença {Id}", id);

            var resultado = await _doencaUseCase.ObterPorIdAsync(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpGet("nome/{nome}")]
        [SwaggerOperation(
            Summary = "Busca doenças por nome",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo as doenças ativas cujo nome corresponde, total ou parcialmente, ao termo informado.
            * **Status 204 (No Content):** Executado com sucesso, porém nenhuma doença ativa corresponde ao termo informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * A busca é case-insensitive e não diferencia maiúsculas de minúsculas.
            * Somente doenças ativas são retornadas por este endpoint.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada com esse nome")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencaByNome(string nome)
        {
            _logger.LogInformation("Buscando doenças por nome {Nome}", nome);

            var resultado = await _doencaUseCase.ObterPorNomeAsync(nome);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpGet("categoria/{idCategoria}")]
        [SwaggerOperation(
            Summary = "Lista doenças por categoria",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo as doenças ativas vinculadas à categoria informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há doenças ativas vinculadas a esta categoria.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Somente doenças ativas são retornadas por este endpoint.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada para esta categoria")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencaByCategoria(int idCategoria)
        {
            _logger.LogInformation("Listando doenças da categoria {IdCategoria}", idCategoria);

            var resultado = await _doencaUseCase.ObterPorCategoriaAsync(idCategoria);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpPost]
        [Authorize(Roles = Roles.SysAdminOuVeterinario)]
        [SwaggerResponse(statusCode: 403, description: "Perfil sem permissão para esta operação")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Cria uma nova doença",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** A doença foi cadastrada com sucesso.
            * **Status 404 (Not Found):** A categoria informada não existe ou está inativa.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados (ex: nome já cadastrado).

            ## Observações:
            * A categoria é opcional; se não for informada, a doença é cadastrada sem vínculo de categoria.
            * Se informada, a categoria precisa existir e estar ativa.
            * A doença é criada sempre com status ativo.
            """
        )]
        [SwaggerRequestExample(typeof(DoencaRequestDto), typeof(DoencaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Doença criada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a doença", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(DoencaResponseSample))]
        public async Task<IActionResult> CreateDoenca(DoencaRequestDto model)
        {
            _logger.LogInformation("Criando doença {Nome}", model.Nome);

            var resultado = await _doencaUseCase.AdicionarAsync(model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            _logger.LogInformation("Doença criada com sucesso: {Id}", resultado.Value?.Id ?? 0);
            return CreatedAtAction(nameof(GetDoencaById), new { id = resultado.Value?.Id ?? 0 }, resultado.Value);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SysAdminOuVeterinario)]
        [SwaggerResponse(statusCode: 403, description: "Perfil sem permissão para esta operação")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Atualiza uma doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A doença foi atualizada com sucesso.
            * **Status 404 (Not Found):** Nenhuma doença ativa foi encontrada com o ID informado, ou a categoria informada não existe ou está inativa.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados.

            ## Observações:
            * Somente doenças ativas podem ser atualizadas.
            * Se uma categoria for informada, ela precisa existir e estar ativa.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença atualizada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a doença", type: typeof(string))]
        public async Task<IActionResult> UpdateDoenca(int id, DoencaRequestDto model)
        {
            _logger.LogInformation("Atualizando doença {Id}", id);

            var resultado = await _doencaUseCase.EditarAsync(id, model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpPut("reativar/{id}")]
        [Authorize(Roles = Roles.SysAdminOuVeterinario)]
        [SwaggerResponse(statusCode: 403, description: "Perfil sem permissão para esta operação")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Reativa uma doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A doença foi reativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma doença inativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao reativar a doença.

            ## Observações:
            * Somente doenças com status inativo podem ser reativadas.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença reativada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a doença", type: typeof(string))]
        public async Task<IActionResult> ReactivateDoenca(int id)
        {
            _logger.LogInformation("Reativando doença {Id}", id);

            var resultado = await _doencaUseCase.ReativarAsync(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.SysAdminOuVeterinario)]
        [SwaggerResponse(statusCode: 403, description: "Perfil sem permissão para esta operação")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Inativa uma doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A doença foi inativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma doença ativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao inativar a doença.

            ## Observações:
            * Esta operação realiza uma exclusão lógica (soft delete); o registro não é removido fisicamente do banco.
            * Uma doença já inativa não pode ser inativada novamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença inativada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a doença", type: typeof(string))]
        public async Task<IActionResult> DeleteDoenca(int id)
        {
            _logger.LogInformation("Inativando doença {Id}", id);

            var resultado = await _doencaUseCase.InativarAsync(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }
    }
}
