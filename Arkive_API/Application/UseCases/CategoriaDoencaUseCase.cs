using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class CategoriaDoencaUseCase : ICategoriaDoencaUseCase
    {
        private readonly ICategoriaDoencaRepository _categoriaDoencaRepository;

        public CategoriaDoencaUseCase(ICategoriaDoencaRepository categoriaDoencaRepository)
        {
            _categoriaDoencaRepository = categoriaDoencaRepository;
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterTodasAsync()
        {
            return await _categoriaDoencaRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterAtivasAsync()
        {
            return await _categoriaDoencaRepository.ObterAtivosAsync();
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterInativasAsync()
        {
            return await _categoriaDoencaRepository.ObterInativosAsync();
        }

        public async Task<CategoriaDoencaEntity?> ObterPorIdAsync(int id)
        {
            return await _categoriaDoencaRepository.ObterPorIdAsync(id);
        }

        public async Task<CategoriaDoencaEntity?> AdicionarAsync(CategoriaDoencaRequestDto dto)
        {
            return await _categoriaDoencaRepository.AdicionarAsync(dto.ToCategoriaDoencaEntity());
        }

        public async Task<CategoriaDoencaEntity?> EditarAsync(int id, CategoriaDoencaRequestDto dto)
        {
            return await _categoriaDoencaRepository.EditarAsync(id, dto.ToCategoriaDoencaEntity());
        }

        public async Task<CategoriaDoencaEntity?> ReativarAsync(int id)
        {
            return await _categoriaDoencaRepository.ReativarAsync(id);
        }

        public async Task<CategoriaDoencaEntity?> InativarAsync(int id)
        {
            return await _categoriaDoencaRepository.InativarAsync(id);
        }
    }
}
