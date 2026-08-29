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
    [Route("api/predisposicoes")]
    [ApiController]
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
            Description = "Retorna todos os vínculos de predisposição entre espécie, raça e doença."
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
            Description = "Retorna um vínculo de predisposição específico pelo seu ID."
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
            Description = "Retorna todas as predisposições vinculadas a uma espécie específica."
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
            Description = "Retorna todas as predisposições vinculadas a uma raça específica."
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
            Description = "Retorna todas as predisposições vinculadas a uma doença específica."
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
        [SwaggerOperation(
            Summary = "Cria vínculo de predisposição",
            Description = "Cadastra um novo vínculo de predisposição entre espécie/raça e doença."
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
        [SwaggerOperation(
            Summary = "Remove vínculo de predisposição",
            Description = "Remove fisicamente o vínculo de predisposição entre espécie/raça e doença."
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
