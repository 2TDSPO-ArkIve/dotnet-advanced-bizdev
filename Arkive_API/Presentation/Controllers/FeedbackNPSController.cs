using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/feedbacks-nps")]
    [ApiController]
    [Authorize]
    [SwaggerResponse(statusCode: 401, description: "Token ausente, inválido ou expirado")]
    [EnableRateLimiting("leitura")]
    public class FeedbackNPSController : ControllerBase
    {
        private readonly IFeedbackNPSUseCase _feedbackNPSUseCase;
        private readonly ILogger<FeedbackNPSController> _logger;

        public FeedbackNPSController(IFeedbackNPSUseCase feedbackNPSUseCase, ILogger<FeedbackNPSController> logger)
        {
            _feedbackNPSUseCase = feedbackNPSUseCase;
            _logger = logger;
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
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(FeedbackNPSResponseListSample))]
        public async Task<IActionResult> GetAllFeedbacks(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS (skip {Skip}, take {Take})", skip, take);

            var resultado = await _feedbackNPSUseCase.ObterTodosAsync(skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
        [SwaggerResponse(statusCode: 200, description: "Feedback retornado com sucesso", type: typeof(GetFeedbackNPSDto))]
        [SwaggerResponse(statusCode: 404, description: "Feedback não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(FeedbackNPSByIdResponseSample))]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            _logger.LogInformation("Buscando feedback NPS {Id}", id);

            var resultado = await _feedbackNPSUseCase.ObterPorIdAsync(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }

        [HttpGet("nota/{nota}")]
        [SwaggerOperation(
            Summary = "Lista feedbacks por nota",
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo os feedbacks com a nota NPS informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há feedbacks com esta nota.
            * **Status 400 (Bad Request):** A nota informada está fora do intervalo permitido (0 a 5), ou ocorreu uma falha durante a consulta.

            ## Observações:
            * A nota deve estar entre 0 e 5; valores fora deste intervalo resultam em 400 (Bad Request).
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta nota")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByNota(int nota, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS com nota {Nota} (skip {Skip}, take {Take})", nota, skip, take);

            var resultado = await _feedbackNPSUseCase.ObterPorNotaAsync(nota, skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este responsável")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByResponsavel(int idResponsavel, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS do responsável {IdResponsavel} (skip {Skip}, take {Take})", idResponsavel, skip, take);

            var resultado = await _feedbackNPSUseCase.ObterPorResponsavelAsync(idResponsavel, skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este animal")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByAnimal(int idAnimal, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS do animal {IdAnimal} (skip {Skip}, take {Take})", idAnimal, skip, take);

            var resultado = await _feedbackNPSUseCase.ObterPorAnimalAsync(idAnimal, skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta clínica")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByClinica(int idClinica, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS da clínica {IdClinica} (skip {Skip}, take {Take})", idClinica, skip, take);

            var resultado = await _feedbackNPSUseCase.ObterPorClinicaAsync(idClinica, skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para este veterinário")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByVeterinario(int idVeterinario, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS do veterinário {IdVeterinario} (skip {Skip}, take {Take})", idVeterinario, skip, take);

            var resultado = await _feedbackNPSUseCase.ObterPorVeterinarioAsync(idVeterinario, skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
            * Ordenação: `ordenarPor` (`id`, `nota` ou `data`) e `direcao` (`asc` ou `desc`). Valor inválido ordena por `id` crescente.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(PageResultModel<IEnumerable<GetFeedbackNPSDto>>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum feedback encontrado para esta data")]
        [SwaggerResponse(statusCode: 400, description: "Formato de data inválido ou erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetFeedbackByData(string data, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            _logger.LogInformation("Listando feedbacks NPS da data {Data} (skip {Skip}, take {Take})", data, skip, take);

            if (!DateTime.TryParse(data, out DateTime dataParsed))
            {
                _logger.LogWarning("Formato de data inválido recebido: {Data}", data);
                return BadRequest("Formato de data inválido. Use o formato yyyy-MM-dd.");
            }

            var resultado = await _feedbackNPSUseCase.ObterPorDataAsync(dataParsed, skip, take, ordenarPor, direcao);

            if (resultado.StatusCode == 204) return NoContent();
            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
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
            _logger.LogInformation("Registrando feedback NPS com nota {Nota}", model.Nota);

            var resultado = await _feedbackNPSUseCase.AdicionarAsync(model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            _logger.LogInformation("Feedback NPS registrado com sucesso: {Id}", resultado.Value?.Id ?? 0);
            return CreatedAtAction(nameof(GetFeedbackById), new { id = resultado.Value?.Id ?? 0 }, resultado.Value);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.SysAdmin)]
        [SwaggerResponse(statusCode: 403, description: "Perfil sem permissão para esta operação")]
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
            _logger.LogInformation("Removendo feedback NPS {Id}", id);

            var resultado = await _feedbackNPSUseCase.DeletarAsync(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);
            return Ok(resultado.Value);
        }
    }
}
