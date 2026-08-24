using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/feedbacks-nps")]
    [ApiController]
    public class FeedbackNPSController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public FeedbackNPSController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os feedbacks NPS",
            Description = "Retorna todos os feedbacks de satisfação registrados."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var resultado = await _context.FeedbackNPS.ToListAsync();

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
            Summary = "Busca feedback NPS por ID",
            Description = "Retorna um feedback de satisfação específico pelo seu ID."
        )]
        [SwaggerResponse(statusCode: 200, description: "Feedback retornado com sucesso", type: typeof(FeedbackNPSEntity))]
        [SwaggerResponse(statusCode: 404, description: "Feedback não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _context.FeedbackNPS
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (feedback is null)
                    return NotFound();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("nota/{nota}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por nota",
            Description = "Retorna todos os feedbacks com a nota NPS informada (0 a 10)."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta nota")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByNota(int nota)
        {
            try
            {
                if (nota < 0 || nota > 10)
                {
                    return BadRequest("Nota inválida, deve estar entre 0 e 10");
                }
                var resultado = await _context.FeedbackNPS
                    .Where(x => x.Nota == nota)
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

        [HttpGet("responsavel/{idResponsavel}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por responsável",
            Description = "Retorna todos os feedbacks vinculados a um responsável específico."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este responsável")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByResponsavel(int idResponsavel)
        {
            try
            {
                var resultado = await _context.FeedbackNPS
                    .Where(x => x.IdResponsavel == idResponsavel)
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

        [HttpGet("animal/{idAnimal}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por animal",
            Description = "Retorna todos os feedbacks vinculados a um animal específico."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este animal")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByAnimal(int idAnimal)
        {
            try
            {
                var resultado = await _context.FeedbackNPS
                    .Where(x => x.IdAnimal == idAnimal)
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

        [HttpGet("clinica/{idClinica}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por clínica",
            Description = "Retorna todos os feedbacks vinculados a uma clínica específica."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta clínica")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByClinica(int idClinica)
        {
            try
            {
                var resultado = await _context.FeedbackNPS
                    .Where(x => x.IdClinica == idClinica)
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

        [HttpGet("veterinario/{idVeterinario}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por veterinário",
            Description = "Retorna todos os feedbacks vinculados a um veterinário específico."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este veterinário")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByVeterinario(int idVeterinario)
        {
            try
            {
                var resultado = await _context.FeedbackNPS
                    .Where(x => x.IdVeterinario == idVeterinario)
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

        [HttpGet("data/{data}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por data",
            Description = "Retorna todos os feedbacks registrados em uma data específica. Formato esperado: yyyy-MM-dd."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta data")]
        [SwaggerResponse(statusCode: 400, description: "Formato de data inválido ou erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByData(string data)
        {
            try
            {
                if (!DateTime.TryParse(data, out DateTime dataParsed))
                    return BadRequest("Formato de data inválido. Use o formato yyyy-MM-dd.");

                var resultado = await _context.FeedbackNPS
                    .Where(x => x.DataFeedback.Date == dataParsed.Date)
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
            Summary = "Registra um novo feedback NPS",
            Description = "Registra um feedback de satisfação vinculado a ao menos um contexto: responsável, animal, clínica, consulta ou veterinário."
        )]
        [SwaggerResponse(statusCode: 201, description: "Feedback registrado com sucesso", type: typeof(FeedbackNPSEntity))]
        [SwaggerResponse(statusCode: 404, description: "Contexto informado não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao registrar o feedback", type: typeof(string))]
        public async Task<IActionResult> CreateFeedback(FeedbackNPSEntity model)
        {
            try
            {

                if (model.IdResponsavel is null && model.IdAnimal is null &&
                    model.IdClinica is null && model.IdConsulta is null &&
                    model.IdVeterinario is null)
                    return BadRequest("Informe ao menos um contexto: responsável, animal, clínica, consulta ou veterinário.");

                // Valida existência dos IDs externos (tabelas gerenciadas pela API Java)
                var responsavel = await _context.Responsavel
                    .FirstOrDefaultAsync(x => x.Id == model.IdResponsavel);

                var animal = await _context.Animal
                    .FirstOrDefaultAsync(x => x.Id == model.IdAnimal);

                var clinica = await _context.Clinica
                    .FirstOrDefaultAsync(x => x.Id == model.IdClinica);

                var consulta = await _context.Consulta
                    .FirstOrDefaultAsync(x => x.Id == model.IdConsulta);

                var veterinario = await _context.Veterinario
                    .FirstOrDefaultAsync(x => x.Id == model.IdVeterinario);

                if (model.IdResponsavel is not null && responsavel is null)
                    return NotFound($"Responsável com ID {model.IdResponsavel} não encontrado.");

                if (model.IdAnimal is not null && animal is null)
                    return NotFound($"Animal com ID {model.IdAnimal} não encontrado.");

                if (model.IdClinica is not null && clinica is null)
                    return NotFound($"Clínica com ID {model.IdClinica} não encontrada.");

                if (model.IdConsulta is not null && consulta is null)
                    return NotFound($"Consulta com ID {model.IdConsulta} não encontrada.");

                if (model.IdVeterinario is not null && veterinario is null)
                    return NotFound($"Veterinário com ID {model.IdVeterinario} não encontrado.");

                _context.FeedbackNPS.Add(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFeedbackById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Sem PUT — feedback NPS é um registro imutável por natureza

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Remove um feedback NPS",
            Description = "Remove fisicamente um registro de feedback NPS do sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Feedback removido com sucesso", type: typeof(FeedbackNPSEntity))]
        [SwaggerResponse(statusCode: 404, description: "Feedback não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao remover o feedback", type: typeof(string))]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var feedback = await _context.FeedbackNPS
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (feedback is null)
                    return NotFound();

                _context.FeedbackNPS.Remove(feedback);
                await _context.SaveChangesAsync();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}