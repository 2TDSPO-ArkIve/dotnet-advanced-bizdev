using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/categorias-doenca")]
    [ApiController]
    public class CategoriaDoencaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CategoriaDoencaController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as categorias de doença",
            Description = "Retorna todas as categorias clínicas cadastradas no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllCategorias()
        {
            try
            {
                var resultado = await _context.CategoriaDoenca.ToListAsync();

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
            Summary = "Lista categorias de doença ativas",
            Description = "Retorna todas as categorias clínicas com status ativo."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetCategoriasAtivas()
        {
            try
            {
                var resultado = await _context.CategoriaDoenca
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
            Summary = "Lista categorias de doença inativas",
            Description = "Retorna todas as categorias clínicas com status inativo (excluídas logicamente)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetCategoriasInativas()
        {
            try
            {
                var resultado = await _context.CategoriaDoenca
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
            Summary = "Busca categoria de doença por ID",
            Description = "Retorna uma categoria clínica específica pelo seu ID, independente do status."
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria retornada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return NotFound();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria uma nova categoria de doença",
            Description = "Cadastra uma nova categoria clínica no sistema."
        )]
        [SwaggerResponse(statusCode: 201, description: "Categoria criada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a categoria", type: typeof(string))]
        public async Task<IActionResult> CreateCategoria(CategoriaDoencaEntity model)
        {
            try
            {
                model.StAtivo = "S";

                _context.CategoriaDoenca.Add(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoriaById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualiza uma categoria de doença",
            Description = "Atualiza os dados de uma categoria clínica ativa existente."
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria atualizada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a categoria", type: typeof(string))]
        public async Task<IActionResult> UpdateCategoria(int id, CategoriaDoencaEntity model)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return NotFound();

                categoria.Nome = model.Nome;

                _context.CategoriaDoenca.Update(categoria);
                await _context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reativar/{id}")]
        [SwaggerOperation(
            Summary = "Reativa uma categoria de doença",
            Description = "Restaura uma categoria clínica previamente inativada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria reativada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a categoria", type: typeof(string))]
        public async Task<IActionResult> ReactivateCategoria(int id)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return NotFound();

                categoria.StAtivo = "S";

                _context.CategoriaDoenca.Update(categoria);
                await _context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Inativa uma categoria de doença",
            Description = "Realiza a exclusão lógica de uma categoria clínica (soft delete)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Categoria inativada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a categoria", type: typeof(string))]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return NotFound();

                categoria.StAtivo = "N";

                _context.CategoriaDoenca.Update(categoria);
                await _context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}