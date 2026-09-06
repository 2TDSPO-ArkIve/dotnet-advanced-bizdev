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
    [Route("api/feedbacks-nps")]
    [ApiController]
    [EnableRateLimiting("leitura")]
    public class FeedbackNPSController : ControllerBase
    {
        private readonly IFeedbackNPSUseCase _feedbackNPSUseCase;

        public FeedbackNPSController(IFeedbackNPSUseCase feedbackNPSUseCase)
        {
            _feedbackNPSUseCase = feedbackNPSUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os feedbacks NPS",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo todos os feedbacks de satisfação registrados.
            * **Status 204 (No Content):** Executado com sucesso, porém a base não possui feedbacks cadastrados.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Esta entidade não possui soft delete; um feedback removido é apagado fisicamente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(FeedbackNPSResponseListSample))]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var resultado = await _feedbackNPSUseCase.ObterTodosAsync();

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna o feedback correspondente ao ID informado.
            * **Status 404 (Not Found):** Nenhum feedback foi encontrado com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Feedback retornado com sucesso", type: typeof(FeedbackNPSEntity))]
        [SwaggerResponse(statusCode: 404, description: "Feedback não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(FeedbackNPSResponseSample))]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _feedbackNPSUseCase.ObterPorIdAsync(id);

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks com a nota NPS informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks com esta nota.
            * **Status 400 (Bad Request):** A nota informada está fora do intervalo permitido (0 a 10), ou ocorreu uma falha durante a consulta.

            ## Observações:
            * A nota deve estar entre 0 e 10; valores fora deste intervalo resultam em 400 (Bad Request).
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta nota")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByNota(int nota)
        {
            try
            {
                var resultado = await _feedbackNPSUseCase.ObterPorNotaAsync(nota);

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks vinculados ao responsável informado.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks vinculados a este responsável.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * O ID de responsável faz referência a uma tabela sincronizada pela API Java; este endpoint não valida sua existência.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este responsável")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByResponsavel(int idResponsavel)
        {
            try
            {
                var resultado = await _feedbackNPSUseCase.ObterPorResponsavelAsync(idResponsavel);

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks vinculados ao animal informado.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks vinculados a este animal.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * O ID de animal faz referência a uma tabela sincronizada pela API Java; este endpoint não valida sua existência.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este animal")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByAnimal(int idAnimal)
        {
            try
            {
                var resultado = await _feedbackNPSUseCase.ObterPorAnimalAsync(idAnimal);

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks vinculados à clínica informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks vinculados a esta clínica.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * O ID de clínica faz referência a uma tabela sincronizada pela API Java; este endpoint não valida sua existência.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta clínica")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByClinica(int idClinica)
        {
            try
            {
                var resultado = await _feedbackNPSUseCase.ObterPorClinicaAsync(idClinica);

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks vinculados ao veterinário informado.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks vinculados a este veterinário.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * O ID de veterinário faz referência a uma tabela sincronizada pela API Java; este endpoint não valida sua existência.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este veterinário")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByVeterinario(int idVeterinario)
        {
            try
            {
                var resultado = await _feedbackNPSUseCase.ObterPorVeterinarioAsync(idVeterinario);

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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks registrados na data informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks registrados nesta data.
            * **Status 400 (Bad Request):** O formato de data informado é inválido, ou ocorreu uma falha durante a consulta.

            ## Observações:
            * O formato esperado para a data é yyyy-MM-dd.
            * A comparação considera apenas a data, ignorando a hora do registro.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<FeedbackNPSEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta data")]
        [SwaggerResponse(statusCode: 400, description: "Formato de data inválido ou erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByData(string data)
        {
            try
            {
                // Parsing de formato de entrada (rota é string) — não é regra de negócio,
                // então permanece no Controller, igual ao original.
                if (!DateTime.TryParse(data, out DateTime dataParsed))
                    return BadRequest("Formato de data inválido. Use o formato yyyy-MM-dd.");

                var resultado = await _feedbackNPSUseCase.ObterPorDataAsync(dataParsed);

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
            Summary = "Registra um novo feedback NPS",
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** O feedback foi registrado com sucesso.
            * **Status 404 (Not Found):** Algum contexto informado (responsável, animal, clínica, consulta ou veterinário) não foi encontrado.
            * **Status 400 (Bad Request):** Nenhum contexto foi informado, a nota está fora do intervalo permitido, ou ocorreu outra falha de validação.

            ## Observações:
            * É obrigatório informar ao menos um contexto: responsável, animal, clínica, consulta ou veterinário.
            * Os IDs de contexto fazem referência a tabelas sincronizadas pela API Java; cada um informado é validado individualmente.
            * A data do feedback é preenchida automaticamente com o momento do registro.
            """
        )]
        [SwaggerRequestExample(typeof(FeedbackNPSRequestDto), typeof(FeedbackNPSRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Feedback registrado com sucesso", type: typeof(FeedbackNPSEntity))]
        [SwaggerResponse(statusCode: 404, description: "Contexto informado não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao registrar o feedback", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(FeedbackNPSResponseSample))]
        public async Task<IActionResult> CreateFeedback(FeedbackNPSRequestDto model)
        {
            try
            {
                var feedback = await _feedbackNPSUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback?.Id ?? 0 }, feedback);
            }
            catch (ContextoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Sem PUT — feedback NPS é um registro imutável por natureza

        [HttpDelete("{id}")]
        [EnableRateLimiting("escrita")]
        [SwaggerOperation(
            Summary = "Remove um feedback NPS",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** O feedback foi removido com sucesso.
            * **Status 404 (Not Found):** Nenhum feedback foi encontrado com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao remover o feedback.

            ## Observações:
            * Esta operação realiza uma exclusão física (hard delete); o registro é removido definitivamente do banco.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Feedback removido com sucesso", type: typeof(FeedbackNPSEntity))]
        [SwaggerResponse(statusCode: 404, description: "Feedback não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao remover o feedback", type: typeof(string))]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackNPSUseCase.DeletarAsync(id);

                if (feedback is null)
                    return NotFound();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}