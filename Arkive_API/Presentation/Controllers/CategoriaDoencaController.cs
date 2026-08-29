using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Doc.Samples;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Presentation.Controllers
{
    [Route("api/categorias-doenca")]
    [ApiController]
    public class CategoriaDoencaController : ControllerBase
    {
        private readonly ICategoriaDoencaUseCase _categoriaDoencaUseCase;

        public CategoriaDoencaController(ICategoriaDoencaUseCase categoriaDoencaUseCase)
        {
            _categoriaDoencaUseCase = categoriaDoencaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as categorias de doença",
            Description = "Retorna todas as categorias clínicas cadastradas no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem de dados retornada com sucesso", type: typeof(IEnumerable<CategoriaDoencaEntity>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma categoria encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao retornar os dados", type: typeof(string))]
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaDoencaResponseListSample))]
        public async Task<IActionResult> GetAllCategorias()
        {
            try
            {
                var resultado = await _categoriaDoencaUseCase.ObterTodasAsync();

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
                var resultado = await _categoriaDoencaUseCase.ObterAtivasAsync();

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
                var resultado = await _categoriaDoencaUseCase.ObterInativasAsync();

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
        [SwaggerResponseExample(statusCode: 200, typeof(CategoriaDoencaResponseSample))]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            try
            {
                var categoria = await _categoriaDoencaUseCase.ObterPorIdAsync(id);

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
        [SwaggerRequestExample(typeof(CategoriaDoencaRequestDto), typeof(CategoriaDoencaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Categoria criada com sucesso", type: typeof(CategoriaDoencaEntity))]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao criar a categoria", type: typeof(string))]
        public async Task<IActionResult> CreateCategoria(CategoriaDoencaRequestDto model)
        {
            try
            {
                var categoria = await _categoriaDoencaUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria?.Id ?? 0 }, categoria);
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
        public async Task<IActionResult> UpdateCategoria(int id, CategoriaDoencaRequestDto model)
        {
            try
            {
                var categoria = await _categoriaDoencaUseCase.EditarAsync(id, model);

                if (categoria is null)
                    return NotFound();

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
                var categoria = await _categoriaDoencaUseCase.ReativarAsync(id);

                if (categoria is null)
                    return NotFound();

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
                var categoria = await _categoriaDoencaUseCase.InativarAsync(id);

                if (categoria is null)
                    return NotFound();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
