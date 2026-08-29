using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class DoencaUseCase : IDoencaUseCase
    {
        private readonly IDoencaRepository _doencaRepository;
        private readonly ICategoriaDoencaRepository _categoriaDoencaRepository;

        public DoencaUseCase(IDoencaRepository doencaRepository, ICategoriaDoencaRepository categoriaDoencaRepository)
        {
            _doencaRepository = doencaRepository;
            _categoriaDoencaRepository = categoriaDoencaRepository;
        }

        public async Task<IEnumerable<DoencaEntity>> ObterTodasAsync()
        {
            return await _doencaRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<DoencaEntity>> ObterAtivasAsync()
        {
            return await _doencaRepository.ObterAtivosAsync();
        }

        public async Task<IEnumerable<DoencaEntity>> ObterInativasAsync()
        {
            return await _doencaRepository.ObterInativosAsync();
        }

        public async Task<DoencaEntity?> ObterPorIdAsync(int id)
        {
            return await _doencaRepository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<DoencaEntity>> ObterPorNomeAsync(string nome)
        {
            return await _doencaRepository.ObterPorNomeAsync(nome);
        }

        public async Task<IEnumerable<DoencaEntity>> ObterPorCategoriaAsync(int idCategoria)
        {
            return await _doencaRepository.ObterPorCategoriaAsync(idCategoria);
        }

        public async Task<DoencaEntity?> AdicionarAsync(DoencaRequestDto dto)
        {
            await ValidarCategoriaAsync(dto.IdCategoria);

            return await _doencaRepository.AdicionarAsync(dto.ToDoencaEntity());
        }

        public async Task<DoencaEntity?> EditarAsync(int id, DoencaRequestDto dto)
        {
            await ValidarCategoriaAsync(dto.IdCategoria);

            return await _doencaRepository.EditarAsync(id, dto.ToDoencaEntity());
        }

        public async Task<DoencaEntity?> ReativarAsync(int id)
        {
            return await _doencaRepository.ReativarAsync(id);
        }

        public async Task<DoencaEntity?> InativarAsync(int id)
        {
            return await _doencaRepository.InativarAsync(id);
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
                throw new CategoriaNaoEncontradaException(idCategoria.Value);
        }
    }
}
