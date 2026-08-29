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
    [Route("api/racas")]
    [ApiController]
    public class RacaController : ControllerBase
    {
        private readonly IRacaUseCase _racaUseCase;

        public RacaController(IRacaUseCase racaUseCase)
        {
            _racaUseCase = racaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as raças",
            Description = "Retorna todas as raças cadastradas, incluindo a espécie vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<RacaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma raça encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(RacaResponseListSample))]
        public async Task<IActionResult> GetAllRacas()
        {
            try
            {
                var resultado = await _racaUseCase.ObterTodasAsync();

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
            Summary = "Lista raças ativas",
            Description = "Retorna todas as raças com status ativo, incluindo a espécie vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<RacaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma raça ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRacasAtivas()
        {
            try
            {
                var resultado = await _racaUseCase.ObterAtivasAsync();

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
            Summary = "Lista raças inativas",
            Description = "Retorna todas as raças com status inativo (excluídas logicamente), incluindo a espécie vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<RacaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma raça inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRacasInativas()
        {
            try
            {
                var resultado = await _racaUseCase.ObterInativasAsync();

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
            Summary = "Busca raça por ID",
            Description = "Retorna uma raça específica pelo seu ID, independente do status, incluindo a espécie vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Raça retornada com sucesso", type: typeof(RacaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Raça não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(RacaResponseSample))]
        public async Task<IActionResult> GetRacaById(int id)
        {
            try
            {
                var raca = await _racaUseCase.ObterPorIdAsync(id);

                if (raca is null)
                    return NotFound();

                return Ok(raca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("especie/{idEspecie}")]
        [SwaggerOperation(
            Summary = "Lista raças por espécie",
            Description = "Retorna todas as raças ativas vinculadas a uma espécie específica."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<RacaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma raça encontrada para esta espécie")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRacasByEspecie(int idEspecie)
        {
            try
            {
                var resultado = await _racaUseCase.ObterPorEspecieAsync(idEspecie);

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
            Summary = "Cria uma nova raça",
            Description = "Cadastra uma nova raça vinculada a uma espécie ativa existente."
        )]
        [SwaggerRequestExample(typeof(RacaRequestDto), typeof(RacaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Raça criada com sucesso", type: typeof(RacaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a raça", type: typeof(string))]
        public async Task<IActionResult> CreateRaca(RacaRequestDto model)
        {
            try
            {
                var raca = await _racaUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetRacaById), new { id = raca?.Id ?? 0 }, raca);
            }
            catch (EspecieNaoEncontradaException ex)
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
            Summary = "Atualiza uma raça",
            Description = "Atualiza os dados de uma raça ativa existente."
        )]
        [SwaggerResponse(statusCode: 200, description: "Raça atualizada com sucesso", type: typeof(RacaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Raça não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a raça", type: typeof(string))]
        public async Task<IActionResult> UpdateRaca(int id, RacaRequestDto model)
        {
            try
            {
                var raca = await _racaUseCase.EditarAsync(id, model);

                if (raca is null)
                    return NotFound();

                return Ok(raca);
            }
            catch (EspecieNaoEncontradaException ex)
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
            Summary = "Reativa uma raça",
            Description = "Restaura uma raça previamente inativada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Raça reativada com sucesso", type: typeof(RacaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Raça não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a raça", type: typeof(string))]
        public async Task<IActionResult> ReactivateRaca(int id)
        {
            try
            {
                var raca = await _racaUseCase.ReativarAsync(id);

                if (raca is null)
                    return NotFound();

                return Ok(raca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Inativa uma raça",
            Description = "Realiza a exclusão lógica de uma raça (soft delete)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Raça inativada com sucesso", type: typeof(RacaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Raça não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a raça", type: typeof(string))]
        public async Task<IActionResult> DeleteRaca(int id)
        {
            try
            {
                var raca = await _racaUseCase.InativarAsync(id);

                if (raca is null)
                    return NotFound();

                return Ok(raca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
