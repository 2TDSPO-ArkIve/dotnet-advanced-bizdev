using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/doencas")]
    [ApiController]
    public class DoencaController : ControllerBase
    {
        private readonly IDoencaUseCase _doencaUseCase;

        public DoencaController(IDoencaUseCase doencaUseCase)
        {
            _doencaUseCase = doencaUseCase;
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
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DoencaResponseListSample))]
        public async Task<IActionResult> GetAllDoencas()
        {
            try
            {
                var resultado = await _doencaUseCase.ObterTodasAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencasAtivas()
        {
            try
            {
                var resultado = await _doencaUseCase.ObterAtivasAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencasInativas()
        {
            try
            {
                var resultado = await _doencaUseCase.ObterInativasAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var doenca = await _doencaUseCase.ObterPorIdAsync(id);

                if (doenca is null)
                    return NotFound();

                return Ok(doenca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var resultado = await _doencaUseCase.ObterPorNomeAsync(nome);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var resultado = await _doencaUseCase.ObterPorCategoriaAsync(idCategoria);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
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
        public async Task<IActionResult> CreateDoenca(DoencaRequestDto model)
        {
            try
            {
                var doenca = await _doencaUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetDoencaById), new { id = doenca?.Id ?? 0 }, doenca);
            }
            catch (CategoriaNaoEncontradaException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
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
            try
            {
                var doenca = await _doencaUseCase.EditarAsync(id, model);

                if (doenca is null)
                    return NotFound();

                return Ok(doenca);
            }
            catch (CategoriaNaoEncontradaException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reativar/{id}")]
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
            try
            {
                var doenca = await _doencaUseCase.ReativarAsync(id);

                if (doenca is null)
                    return NotFound();

                return Ok(doenca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
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
            try
            {
                var doenca = await _doencaUseCase.InativarAsync(id);

                if (doenca is null)
                    return NotFound();

                return Ok(doenca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}