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
    [Route("api/doencas")]
    [ApiController]
    public class DoencaController : ControllerBase
    {
        private readonly IDoencaUseCase _doencaUseCase;

        public DoencaController(IDoencaUseCase doencaUseCase)
        {
            _doencaUseCase = doencaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as doenças",
            Description = "Retorna todas as doenças cadastradas, incluindo a categoria clínica vinculada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<DoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma doença encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(DoencaResponseListSample))]
        public async Task<IActionResult> GetAllDoencas()
        {
            try
            {
                var resultado = await _doencaUseCase.ObterTodasAsync();

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
                var resultado = await _doencaUseCase.ObterAtivasAsync();

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
                var resultado = await _doencaUseCase.ObterInativasAsync();

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
        [SwaggerResponseExample(statusCode: 200, typeof(DoencaResponseSample))]
        public async Task<IActionResult> GetDoencaById(int id)
        {
            try
            {
                var doenca = await _doencaUseCase.ObterPorIdAsync(id);

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
                var resultado = await _doencaUseCase.ObterPorNomeAsync(nome);

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
                var resultado = await _doencaUseCase.ObterPorCategoriaAsync(idCategoria);

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
        [SwaggerRequestExample(typeof(DoencaRequestDto), typeof(DoencaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Doença criada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Categoria informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a doença", type: typeof(string))]
        public async Task<IActionResult> CreateDoenca(DoencaRequestDto model)
        {
            try
            {
                var doenca = await _doencaUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetDoencaById), new { id = doenca?.Id ?? 0 }, doenca);
            }
            catch (CategoriaNaoEncontradaException ex)
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
            Summary = "Atualiza uma doença",
            Description = "Atualiza os dados de uma doença ativa existente no catálogo."
        )]
        [SwaggerResponse(statusCode: 200, description: "Doença atualizada com sucesso", type: typeof(DoencaEntity))]
        [SwaggerResponse(statusCode: 404, description: "Doença não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao atualizar a doença", type: typeof(string))]
        public async Task<IActionResult> UpdateDoenca(int id, DoencaRequestDto model)
        {
            try
            {
                var doenca = await _doencaUseCase.EditarAsync(id, model);

                if (doenca is null)
                    return NotFound();

                return Ok(doenca);
            }
            catch (CategoriaNaoEncontradaException ex)
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
                var doenca = await _doencaUseCase.ReativarAsync(id);

                if (doenca is null)
                    return NotFound();

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
                var doenca = await _doencaUseCase.InativarAsync(id);

                if (doenca is null)
                    return NotFound();

                return Ok(doenca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
