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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo todas as raças cadastradas, ativas e inativas.
            * **Status 204 (No Content):** Executado com sucesso, porém a base não possui raças cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Espécie**).
            * Este endpoint não filtra por status; use `/ativos` ou `/inativos` para isso.
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as raças com status ativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há raças ativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Espécie**).
            * Raças inativas (excluídas logicamente) não aparecem neste retorno.
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo apenas as raças com status inativo.
            * **Status 204 (No Content):** Executado com sucesso, porém não há raças inativas cadastradas.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Espécie**).
            * Raças inativas são registros excluídos logicamente (soft delete), não removidos fisicamente.
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna a raça correspondente ao ID informado.
            * **Status 404 (Not Found):** Nenhuma raça foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Os dados incluem a entidade relacionada (**Espécie**).
            * A busca por ID retorna a raça independente do status (ativa ou inativa).
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** Retorna uma lista contendo as raças ativas vinculadas à espécie informada.
            * **Status 204 (No Content):** Executado com sucesso, porém não há raças ativas vinculadas a esta espécie.
            * **Status 400 (Bad Request):** Ocorreu uma falha durante a consulta (ex: erro de conexão com o banco).

            ## Observações:
            * Somente raças ativas são retornadas por este endpoint.
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 201 (Created):** A raça foi cadastrada com sucesso.
            * **Status 404 (Not Found):** A espécie informada não existe ou está inativa.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados (ex: raça já cadastrada para esta espécie, porte inválido).

            ## Observações:
            * A espécie é obrigatória e precisa existir e estar ativa.
            * O campo Porte, quando informado, aceita apenas PEQUENO, MEDIO ou GRANDE.
            * A raça é criada sempre com status ativo.
            """
        )]
        [SwaggerRequestExample(typeof(RacaRequestDto), typeof(RacaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Raça criada com sucesso", type: typeof(RacaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Espécie informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a raça", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 201, typeof(RacaResponseSample))]
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A raça foi atualizada com sucesso.
            * **Status 404 (Not Found):** Nenhuma raça ativa foi encontrada com o ID informado, ou a espécie informada não existe ou está inativa.
            * **Status 400 (Bad Request):** Ocorreu uma falha de validação ou ao gravar os dados.

            ## Observações:
            * Somente raças ativas podem ser atualizadas.
            * A espécie informada é sempre revalidada nesta operação, mesmo que não tenha sido alterada.
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A raça foi reativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma raça inativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao reativar a raça.

            ## Observações:
            * Somente raças com status inativo podem ser reativadas.
            """
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
            Description = """
            ## Informações do Retorno:
            * **Status 200 (OK):** A raça foi inativada com sucesso.
            * **Status 404 (Not Found):** Nenhuma raça ativa foi encontrada com o ID informado.
            * **Status 400 (Bad Request):** Ocorreu uma falha ao inativar a raça.

            ## Observações:
            * Esta operação realiza uma exclusão lógica (soft delete); o registro não é removido fisicamente do banco.
            * Uma raça já inativa não pode ser inativada novamente.
            """
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