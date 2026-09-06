using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/predisposicoes")]
    [ApiController]
    [EnableRateLimiting("leitura")]
    public class PredisposicaoController : ControllerBase
    {
        private readonly IPredisposicaoUseCase _predisposicaoUseCase;

        public PredisposicaoController(IPredisposicaoUseCase predisposicaoUseCase)
        {
            _predisposicaoUseCase = predisposicaoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as predisposições",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo todos os vínculos de predisposição cadastrados.
            * **Status 204 (No Content):** Executado com sucesso, porém a base não possui predisposições cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Espécie**, **Raça** e **Doença**, esta última com sua **Categoria**).
            * Esta entidade não possui soft delete; um vínculo removido é apagado fisicamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<PredisposicaoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma predisposição encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(PredisposicaoResponseListSample))]
        public async Task<IActionResult> GetAllPredisposicoes()
        {
            try
            {
                var resultado = await _predisposicaoUseCase.ObterTodasAsync();

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
            Summary = "Busca predisposição por ID",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna o vínculo de predisposição correspondente ao ID informado.
            * **Status 404 (Not Found):** Nenhum vínculo foi encontrado com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem as entidades relacionadas (**Espécie**, **Raça** e **Doença**, esta última com sua **Categoria**).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Predisposição retornada com sucesso", type: typeof(PredisposicaoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Predisposição não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(PredisposicaoResponseSample))]
        public async Task<IActionResult> GetPredisposicaoById(int id)
        {
            try
            {
                var predisposicao = await _predisposicaoUseCase.ObterPorIdAsync(id);

                if (predisposicao is null)
                    return NotFound();

                return Ok(predisposicao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("especie/{idEspecie}")]
        [SwaggerOperation(
            Summary = "Lista predisposições por espécie",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os vínculos de predisposição associados à espécie informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há predisposições associadas a esta espécie.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Este filtro não valida se o ID de espécie informado existe; caso não exista, o retorno será uma lista vazia (204).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<PredisposicaoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma predisposição encontrada para esta espécie")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetPredisposicaoByEspecie(int idEspecie)
        {
            try
            {
                var resultado = await _predisposicaoUseCase.ObterPorEspecieAsync(idEspecie);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("raca/{idRaca}")]
        [SwaggerOperation(
            Summary = "Lista predisposições por raça",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os vínculos de predisposição associados à raça informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há predisposições associadas a esta raça.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Este filtro não valida se o ID de raça informado existe; caso não exista, o retorno será uma lista vazia (204).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<PredisposicaoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma predisposição encontrada para esta raça")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetPredisposicaoByRaca(int idRaca)
        {
            try
            {
                var resultado = await _predisposicaoUseCase.ObterPorRacaAsync(idRaca);

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("doenca/{idDoenca}")]
        [SwaggerOperation(
            Summary = "Lista predisposições por doença",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os vínculos de predisposição associados à doença informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há predisposições associadas a esta doença.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Este filtro não valida se o ID de doença informado existe; caso não exista, o retorno será uma lista vazia (204).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<PredisposicaoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma predisposição encontrada para esta doença")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetPredisposicaoByDoenca(int idDoenca)
        {
            try
            {
                var resultado = await _predisposicaoUseCase.ObterPorDoencaAsync(idDoenca);

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
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Cria vínculo de predisposição",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** O vínculo de predisposição foi cadastrado com sucesso.
            * **Status 404 (Not Found):** A espécie, a raça ou a doença informada não existe ou está inativa.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados (ex: vínculo duplicado).

            ## Observações:
            * Espécie e Doença são obrigatórias e precisam existir e estar ativas.
            * Raça é opcional; quando informada, também precisa existir e estar ativa.
            * Não existe endpoint de atualização (PUT); para alterar um vínculo, remova o antigo e crie um novo.
            """
        )]
        [SwaggerRequestExample(typeof(PredisposicaoRequestDto), typeof(PredisposicaoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Predisposição criada com sucesso", type: typeof(PredisposicaoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie, raça ou doença informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a predisposição", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(PredisposicaoResponseSample))]
        public async Task<IActionResult> CreatePredisposicao(PredisposicaoRequestDto model)
        {
            try
            {
                var predisposicao = await _predisposicaoUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetPredisposicaoById), new { id = predisposicao?.Id ?? 0 }, predisposicao);
            }
            catch (EspecieNaoEncontradaException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RacaNaoEncontradaException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DoencaNaoEncontradaException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Sem PUT — conforme PRD §16.3.5:
        // "o PUT pode ser substituído por remover o vínculo antigo e criar um novo"

        [HttpDelete("{id}")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Remove vínculo de predisposição",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** O vínculo de predisposição foi removido com sucesso.
            * **Status 404 (Not Found):** Nenhum vínculo foi encontrado com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao remover o vínculo.

            ## Observações:
            * Esta operação realiza uma exclusão física (hard delete); o registro é removido definitivamente do banco.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Predisposição removida com sucesso", type: typeof(PredisposicaoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Predisposição não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao remover a predisposição", type: typeof(string))]
        public async Task<IActionResult> DeletePredisposicao(int id)
        {
            try
            {
                var predisposicao = await _predisposicaoUseCase.DeletarAsync(id);

                if (predisposicao is null)
                    return NotFound();

                return Ok(predisposicao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}