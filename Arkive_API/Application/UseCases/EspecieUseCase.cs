using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class EspecieUseCase : IEspecieUseCase
    {
        private readonly IEspecieRepository _especieRepository;

        public EspecieUseCase(IEspecieRepository especieRepository)
        {
            _especieRepository = especieRepository;
        }

        public async Task<IEnumerable<EspecieEntity>> ObterTodasAsync()
        {
            return await _especieRepository.ObterTodosAsync();
        }

        public async Task<IEnumerable<EspecieEntity>> ObterAtivasAsync()
        {
            return await _especieRepository.ObterAtivosAsync();
        }

        public async Task<IEnumerable<EspecieEntity>> ObterInativasAsync()
        {
            return await _especieRepository.ObterInativosAsync();
        }

        public async Task<EspecieEntity?> ObterPorIdAsync(int id)
        {
            return await _especieRepository.ObterPorIdAsync(id);
        }

        public async Task<EspecieEntity?> AdicionarAsync(EspecieRequestDto dto)
        {
            return await _especieRepository.AdicionarAsync(dto.ToEspecieEntity());
        }

        public async Task<EspecieEntity?> EditarAsync(int id, EspecieRequestDto dto)
        {
            return await _especieRepository.EditarAsync(id, dto.ToEspecieEntity());
        }

        public async Task<EspecieEntity?> ReativarAsync(int id)
        {
            return await _especieRepository.ReativarAsync(id);
        }

        public async Task<EspecieEntity?> InativarAsync(int id)
        {
            return await _especieRepository.InativarAsync(id);
        }
    }
}
