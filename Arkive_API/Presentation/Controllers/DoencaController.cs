using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/doencas")]
    [ApiController]
    public class DoencaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public DoencaController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as doenças",
            Description = "Retorna todas as doenças cadastradas, incluindo a categoria clínica vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllDoencas()
        {
            try
            {
                var resultado = await _context.Doenca
                    .Include(x => x.Categoria)
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

        [HttpGet("ativos")]
        [SwaggerOperation(
            Summary = "Lista doenças ativas",
            Description = "Retorna todas as doenças com status ativo, incluindo a categoria clínica vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencasAtivas()
        {
            try
            {
                var resultado = await _context.Doenca
                    .Include(x => x.Categoria)
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
            Summary = "Lista doenças inativas",
            Description = "Retorna todas as doenças com status inativo (excluídas logicamente), incluindo a categoria clínica vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencasInativas()
        {
            try
            {
                var resultado = await _context.Doenca
                    .Include(x => x.Categoria)
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
            Summary = "Busca doença por ID",
            Description = "Retorna uma doença específica pelo seu ID, independente do status, incluindo a categoria clínica vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença retornada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencaById(int id)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Include(x => x.Categoria)
                    .FirstOrDefaultAsync(x => x.Id == id);

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
            Description = "Retorna todas as doenças ativas cujo nome contenha o termo informado (busca parcial)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada com esse nome")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencaByNome(string nome)
        {
            try
            {
                var resultado = await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S" && x.Nome.ToLower().Contains(nome.ToLower()))
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

        [HttpGet("categoria/{idCategoria}")]
        [SwaggerOperation(
            Summary = "Lista doenças por categoria",
            Description = "Retorna todas as doenças ativas vinculadas a uma categoria clínica específica."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada para esta categoria")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetDoencaByCategoria(int idCategoria)
        {
            try
            {
                var resultado = await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S" && x.IdCategoria == idCategoria)
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
            Summary = "Cria uma nova doença",
            Description = "Cadastra uma nova doença no catálogo clínico. A categoria é opcional."
        )]
        [SwaggerResponse(statusCode: 201, description: "Doença criada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a doença", type: typeof(string))]
        public async Task<IActionResult> CreateDoenca(DoencaEntity model)
        {
            try
            {
                if (model.IdCategoria is not null)
                {
                    var categoria = await _context.CategoriaDoenca
                        .FirstOrDefaultAsync(x => x.Id == model.IdCategoria && x.StAtivo == "S");

                    if (categoria is null)
                        return NotFound($"Categoria com ID {model.IdCategoria} não encontrada.");
                }

                model.StAtivo = "S";

                _context.Doenca.Add(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDoencaById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualiza uma doença",
            Description = "Atualiza os dados de uma doença ativa existente no catálogo."
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença atualizada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a doença", type: typeof(string))]
        public async Task<IActionResult> UpdateDoenca(int id, DoencaEntity model)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (doenca is null)
                    return NotFound();

                if (model.IdCategoria is not null)
                {
                    var categoria = await _context.CategoriaDoenca
                        .FirstOrDefaultAsync(x => x.Id == model.IdCategoria && x.StAtivo == "S");

                    if (categoria is null)
                        return NotFound($"Categoria com ID {model.IdCategoria} não encontrada.");
                }

                doenca.Nome = model.Nome;
                doenca.IdCategoria = model.IdCategoria;
                doenca.Descricao = model.Descricao;
                doenca.CID = model.CID;
                doenca.Sintomas = model.Sintomas;

                _context.Doenca.Update(doenca);
                await _context.SaveChangesAsync();

                return Ok(doenca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reativar/{id}")]
        [SwaggerOperation(
            Summary = "Reativa uma doença",
            Description = "Restaura uma doença previamente inativada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença reativada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao reativar a doença", type: typeof(string))]
        public async Task<IActionResult> ReactivateDoenca(int id)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (doenca is null)
                    return NotFound();

                doenca.StAtivo = "S";

                _context.Doenca.Update(doenca);
                await _context.SaveChangesAsync();

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
            Description = "Realiza a exclusão lógica de uma doença (soft delete)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença inativada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao inativar a doença", type: typeof(string))]
        public async Task<IActionResult> DeleteDoenca(int id)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (doenca is null)
                    return NotFound();

                doenca.StAtivo = "N";

                _context.Doenca.Update(doenca);
                await _context.SaveChangesAsync();

                return Ok(doenca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}