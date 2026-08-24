using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/predisposicoes")]
    [ApiController]
    public class PredisposicaoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PredisposicaoController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as predisposições",
            Description = "Retorna todos os vínculos de predisposição entre espécie, raça e doença."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<PredisposicaoEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma predisposição encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllPredisposicoes()
        {
            try
            {
                var resultado = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
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
            Summary = "Busca predisposição por ID",
            Description = "Retorna um vínculo de predisposição específico pelo seu ID."
        )]
        [SwaggerResponse(statusCode: 200, description: "Predisposição retornada com sucesso", type: typeof(PredisposicaoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Predisposição não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetPredisposicaoById(int id)
        {
            try
            {
                var predisposicao = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
                    .FirstOrDefaultAsync(x => x.Id == id);

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
                var resultado = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
                    .Where(x => x.IdEspecie == idEspecie)
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
                var resultado = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
                    .Where(x => x.IdRaca == idRaca)
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
                var resultado = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
                    .Where(x => x.IdDoenca == idDoenca)
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

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria vínculo de predisposição",
            Description = "Cadastra um novo vínculo de predisposição entre espécie/raça e doença."
        )]
        [SwaggerResponse(statusCode: 201, description: "Predisposição criada com sucesso", type: typeof(PredisposicaoEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie, raça ou doença informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a predisposição", type: typeof(string))]
        public async Task<IActionResult> CreatePredisposicao(PredisposicaoEntity model)
        {
            try
            {
                var especie = await _context.Especie
                    .FirstOrDefaultAsync(x => x.Id == model.IdEspecie && x.StAtivo == "S");

                if (especie is null)
                    return NotFound($"Espécie com ID {model.IdEspecie} não encontrada.");

                if (model.IdRaca is not null)
                {
                    var raca = await _context.Raca
                        .FirstOrDefaultAsync(x => x.Id == model.IdRaca && x.StAtivo == "S");

                    if (raca is null)
                        return NotFound($"Raça com ID {model.IdRaca} não encontrada.");
                }

                var doenca = await _context.Doenca
                    .FirstOrDefaultAsync(x => x.Id == model.IdDoenca && x.StAtivo == "S");

                if (doenca is null)
                    return NotFound($"Doença com ID {model.IdDoenca} não encontrada.");

                _context.Predisposicao.Add(model);
                await _context.SaveChangesAsync();

                var resultado = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                return CreatedAtAction(nameof(GetPredisposicaoById), new { id = model.Id }, resultado);
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
                var predisposicao = await _context.Predisposicao
                    .Include(x => x.Especie)
                    .Include(x => x.Raca)
                    .Include(x => x.Doenca)
                        .ThenInclude(d => d.Categoria)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (predisposicao is null)
                    return NotFound();

                _context.Predisposicao.Remove(predisposicao);
                await _context.SaveChangesAsync();

                return Ok(predisposicao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}