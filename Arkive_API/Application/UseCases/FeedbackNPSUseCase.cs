using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;

namespace Arkive_API.Application.UseCases
{
    public class FeedbackNPSUseCase : IFeedbackNPSUseCase
    {
        private readonly IFeedbackNPSRepository _feedbackNPSRepository;

        public FeedbackNPSUseCase(IFeedbackNPSRepository feedbackNPSRepository)
        {
            _feedbackNPSRepository = feedbackNPSRepository;
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterTodosAsync()
        {
            return await _feedbackNPSRepository.ObterTodosAsync();
        }

        public async Task<FeedbackNPSEntity?> ObterPorIdAsync(int id)
        {
            return await _feedbackNPSRepository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorNotaAsync(int nota)
        {
            if (nota < 0 || nota > 10)
                throw new ArgumentOutOfRangeException(nameof(nota), "Nota inválida, deve estar entre 0 e 10");

            return await _feedbackNPSRepository.ObterPorNotaAsync(nota);
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorResponsavelAsync(int idResponsavel)
        {
            return await _feedbackNPSRepository.ObterPorResponsavelAsync(idResponsavel);
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorAnimalAsync(int idAnimal)
        {
            return await _feedbackNPSRepository.ObterPorAnimalAsync(idAnimal);
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorClinicaAsync(int idClinica)
        {
            return await _feedbackNPSRepository.ObterPorClinicaAsync(idClinica);
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorVeterinarioAsync(int idVeterinario)
        {
            return await _feedbackNPSRepository.ObterPorVeterinarioAsync(idVeterinario);
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorDataAsync(DateTime data)
        {
            return await _feedbackNPSRepository.ObterPorDataAsync(data);
        }

        public async Task<FeedbackNPSEntity?> AdicionarAsync(FeedbackNPSRequestDto dto)
        {
            if (dto.IdResponsavel is null && dto.IdAnimal is null &&
                dto.IdClinica is null && dto.IdConsulta is null &&
                dto.IdVeterinario is null)
                throw new ArgumentException("Informe ao menos um contexto: responsável, animal, clínica, consulta ou veterinário.");

            if (dto.IdResponsavel is not null && !await _feedbackNPSRepository.ResponsavelExisteAsync(dto.IdResponsavel.Value))
                throw new ContextoNaoEncontradoException($"Responsável com ID {dto.IdResponsavel} não encontrado.");

            if (dto.IdAnimal is not null && !await _feedbackNPSRepository.AnimalExisteAsync(dto.IdAnimal.Value))
                throw new ContextoNaoEncontradoException($"Animal com ID {dto.IdAnimal} não encontrado.");

            if (dto.IdClinica is not null && !await _feedbackNPSRepository.ClinicaExisteAsync(dto.IdClinica.Value))
                throw new ContextoNaoEncontradoException($"Clínica com ID {dto.IdClinica} não encontrada.");

            if (dto.IdConsulta is not null && !await _feedbackNPSRepository.ConsultaExisteAsync(dto.IdConsulta.Value))
                throw new ContextoNaoEncontradoException($"Consulta com ID {dto.IdConsulta} não encontrada.");

            if (dto.IdVeterinario is not null && !await _feedbackNPSRepository.VeterinarioExisteAsync(dto.IdVeterinario.Value))
                throw new ContextoNaoEncontradoException($"Veterinário com ID {dto.IdVeterinario} não encontrado.");

            return await _feedbackNPSRepository.AdicionarAsync(dto.ToFeedbackNPSEntity());
        }

        public async Task<FeedbackNPSEntity?> DeletarAsync(int id)
        {
            return await _feedbackNPSRepository.DeletarAsync(id);
        }
    }
}
