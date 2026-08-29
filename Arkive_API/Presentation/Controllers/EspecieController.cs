using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/especies")]
    [ApiController]
    public class EspecieController : ControllerBase
    {
        private readonly IEspecieUseCase _especieUseCase;

        public EspecieController(IEspecieUseCase especieUseCase)
        {
            _especieUseCase = especieUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as espécies",
            Description = "Retorna todas as espécies cadastradas no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(EspecieResponseListSample))]
        public async Task<IActionResult> GetAllEspecies()
        {
            try
            {
                var resultado = await _especieUseCase.ObterTodasAsync();

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
            Summary = "Lista espécies ativas",
            Description = "Retorna todas as espécies com status ativo."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetEspeciesAtivas()
        {
            try
            {
                var resultado = await _especieUseCase.ObterAtivasAsync();

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
            Summary = "Lista espécies inativas",
            Description = "Retorna todas as espécies com status inativo (excluídas logicamente)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetEspeciesInativas()
        {
            try
            {
                var resultado = await _especieUseCase.ObterInativasAsync();

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
            Summary = "Busca espécie por ID",
            Description = "Retorna uma espécie específica pelo seu ID, independente do status."
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie retornada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(EspecieResponseSample))]
        public async Task<IActionResult> GetEspecieById(int id)
        {
            try
            {
                var especie = await _especieUseCase.ObterPorIdAsync(id);

                if (especie is null)
                    return NotFound();

                return Ok(especie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria uma nova espécie",
            Description = "Cadastra uma nova espécie no sistema."
        )]
        [SwaggerRequestExample(typeof(EspecieRequestDto), typeof(EspecieRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Espécie criada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a espécie", type: typeof(string))]
        public async Task<IActionResult> CreateEspecie(EspecieRequestDto model)
        {
            try
            {
                var especie = await _especieUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetEspecieById), new { id = especie?.Id ?? 0 }, especie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualiza uma espécie",
            Description = "Atualiza os dados de uma espécie ativa existente."
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie atualizada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a espécie", type: typeof(string))]
        public async Task<IActionResult> UpdateEspecie(int id, EspecieRequestDto model)
        {
            try
            {
                var especie = await _especieUseCase.EditarAsync(id, model);

                if (especie is null)
                    return NotFound();

                return Ok(especie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reativar/{id}")]
        [SwaggerOperation(
            Summary = "Reativa uma espécie",
            Description = "Restaura uma espécie previamente inativada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie reativada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a espécie", type: typeof(string))]
        public async Task<IActionResult> ReactivateEspecie(int id)
        {
            try
            {
                var especie = await _especieUseCase.ReativarAsync(id);

                if (especie is null)
                    return NotFound();

                return Ok(especie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Inativa uma espécie",
            Description = "Realiza a exclusão lógica de uma espécie (soft delete)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Espécie inativada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a espécie", type: typeof(string))]
        public async Task<IActionResult> DeleteEspecie(int id)
        {
            try
            {
                var especie = await _especieUseCase.InativarAsync(id);

                if (especie is null)
                    return NotFound();

                return Ok(especie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
