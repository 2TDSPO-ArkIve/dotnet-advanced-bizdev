using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.UseCases
{
    public class DoencaUseCase : IDoencaUseCase
    {
        private readonly IDoencaRepository _doencaRepository;
        private readonly ICategoriaDoencaRepository _categoriaDoencaRepository;
        private readonly ILogger<DoencaUseCase> _logger;

        public DoencaUseCase(IDoencaRepository doencaRepository, ICategoriaDoencaRepository categoriaDoencaRepository, ILogger<DoencaUseCase> logger)
        {
            _doencaRepository = doencaRepository;
            _categoriaDoencaRepository = categoriaDoencaRepository;
            _logger = logger;
        }

        public async Task<PageResultModel<IEnumerable<DoencaEntity>>> ObterTodasAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo doenças (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _doencaRepository.ObterTodosAsync(s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças");
                throw;
            }
        }

        public async Task<PageResultModel<IEnumerable<DoencaEntity>>> ObterAtivasAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo doenças ativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _doencaRepository.ObterAtivosAsync(s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças ativas");
                throw;
            }
        }

        public async Task<PageResultModel<IEnumerable<DoencaEntity>>> ObterInativasAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo doenças inativas (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _doencaRepository.ObterInativosAsync(s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças inativas");
                throw;
            }
        }

        public async Task<DoencaEntity?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo doença {Id}", id);

            try
            {
                return await _doencaRepository.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doença {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<DoencaEntity>> ObterPorNomeAsync(string nome)
        {
            _logger.LogInformation("Obtendo doenças por nome {Nome}", nome);

            try
            {
                return await _doencaRepository.ObterPorNomeAsync(nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças por nome {Nome}", nome);
                throw;
            }
        }

        public async Task<IEnumerable<DoencaEntity>> ObterPorCategoriaAsync(int idCategoria)
        {
            _logger.LogInformation("Obtendo doenças da categoria {IdCategoria}", idCategoria);

            try
            {
                return await _doencaRepository.ObterPorCategoriaAsync(idCategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter doenças da categoria {IdCategoria}", idCategoria);
                throw;
            }
        }

        public async Task<DoencaEntity?> AdicionarAsync(DoencaRequestDto dto)
        {
            _logger.LogInformation("Adicionando doença {Nome}", dto.Nome);

            await ValidarCategoriaAsync(dto.IdCategoria);

            try
            {
                return await _doencaRepository.AdicionarAsync(dto.ToDoencaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar doença {Nome}", dto.Nome);
                throw;
            }
        }

        public async Task<DoencaEntity?> EditarAsync(int id, DoencaRequestDto dto)
        {
            _logger.LogInformation("Editando doença {Id}", id);

            await ValidarCategoriaAsync(dto.IdCategoria);

            try
            {
                return await _doencaRepository.EditarAsync(id, dto.ToDoencaEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar doença {Id}", id);
                throw;
            }
        }

        public async Task<DoencaEntity?> ReativarAsync(int id)
        {
            _logger.LogInformation("Reativando doença {Id}", id);

            try
            {
                return await _doencaRepository.ReativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reativar doença {Id}", id);
                throw;
            }
        }

        public async Task<DoencaEntity?> InativarAsync(int id)
        {
            _logger.LogInformation("Inativando doença {Id}", id);

            try
            {
                return await _doencaRepository.InativarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar doença {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Regra de negócio: se uma categoria for informada, ela precisa existir e estar ativa.
        /// Categoria é opcional (IdCategoria é nullable), então null não gera erro.
        /// </summary>
        private async Task ValidarCategoriaAsync(int? idCategoria)
        {
            if (idCategoria is null)
                return;

            var categoria = await _categoriaDoencaRepository.ObterPorIdAsync(idCategoria.Value);

            if (categoria is null || categoria.StAtivo != "S")
            {
                _logger.LogWarning("Categoria {IdCategoria} inválida ou inativa", idCategoria.Value);
                throw new CategoriaNaoEncontradaException(idCategoria.Value);
            }
        }
    }
}
