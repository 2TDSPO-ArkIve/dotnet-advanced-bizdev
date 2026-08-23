using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/especies")]
    [ApiController]
    public class EspecieController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public EspecieController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as espécies",
            Description = "Retorna todas as espécies cadastradas no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<EspecieEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma espécie encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllEspecies()
        {
            try
            {
                var resultado = await _context.Especie.ToListAsync();

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
                var resultado = await _context.Especie
                    .Where(x => x.StAtivo == "S")
                    .ToListAsync();

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
                var resultado = await _context.Especie
                    .Where(x => x.StAtivo == "N")
                    .ToListAsync();

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
        public async Task<IActionResult> GetEspecieById(int id)
        {
            try
            {
                var especie = await _context.Especie
                    .FirstOrDefaultAsync(x => x.Id == id);

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
        [SwaggerResponse(statusCode: 201, description: "Espécie criada com sucesso", type: typeof(EspecieEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a espécie", type: typeof(string))]
        public async Task<IActionResult> CreateEspecie(EspecieEntity model)
        {
            try
            {
                model.StAtivo = "S";

                _context.Especie.Add(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEspecieById), new { id = model.Id }, model);
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
        public async Task<IActionResult> UpdateEspecie(int id, EspecieEntity model)
        {
            try
            {
                var especie = await _context.Especie
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (especie is null)
                    return NotFound();

                especie.Especie = model.Especie;

                _context.Especie.Update(especie);
                await _context.SaveChangesAsync();

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
                var especie = await _context.Especie
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (especie is null)
                    return NotFound();

                especie.StAtivo = "S";

                _context.Especie.Update(especie);
                await _context.SaveChangesAsync();

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
                var especie = await _context.Especie
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (especie is null)
                    return NotFound();

                especie.StAtivo = "N";

                _context.Especie.Update(especie);
                await _context.SaveChangesAsync();

                return Ok(especie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}