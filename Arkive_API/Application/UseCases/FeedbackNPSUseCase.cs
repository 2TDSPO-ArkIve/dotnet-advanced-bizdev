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
        private readonly ILogger<FeedbackNPSUseCase> _logger;

        public FeedbackNPSUseCase(IFeedbackNPSRepository feedbackNPSRepository, ILogger<FeedbackNPSUseCase> logger)
        {
            _feedbackNPSRepository = feedbackNPSRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterTodosAsync(int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS (skip {Skip}, take {Take})", skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterTodosAsync(s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS");
                throw;
            }
        }

        public async Task<FeedbackNPSEntity?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Obtendo feedback NPS {Id}", id);

            try
            {
                return await _feedbackNPSRepository.ObterPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedback NPS {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorNotaAsync(int nota, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS com nota {Nota} (skip {Skip}, take {Take})", nota, skip, take);

            if (nota < 0 || nota > 10)
            {
                _logger.LogWarning("Nota inválida recebida: {Nota}", nota);
                throw new ArgumentOutOfRangeException(nameof(nota), "Nota inválida, deve estar entre 0 e 10");
            }

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterPorNotaAsync(nota, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS com nota {Nota}", nota);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorResponsavelAsync(int idResponsavel, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS do responsável {IdResponsavel} (skip {Skip}, take {Take})", idResponsavel, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterPorResponsavelAsync(idResponsavel, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS do responsável {IdResponsavel}", idResponsavel);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorAnimalAsync(int idAnimal, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS do animal {IdAnimal} (skip {Skip}, take {Take})", idAnimal, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterPorAnimalAsync(idAnimal, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS do animal {IdAnimal}", idAnimal);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorClinicaAsync(int idClinica, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS da clínica {IdClinica} (skip {Skip}, take {Take})", idClinica, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterPorClinicaAsync(idClinica, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS da clínica {IdClinica}", idClinica);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorVeterinarioAsync(int idVeterinario, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS do veterinário {IdVeterinario} (skip {Skip}, take {Take})", idVeterinario, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterPorVeterinarioAsync(idVeterinario, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS do veterinário {IdVeterinario}", idVeterinario);
                throw;
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorDataAsync(DateTime data, int skip = 0, int take = 50)
        {
            _logger.LogInformation("Obtendo feedbacks NPS da data {Data:yyyy-MM-dd} (skip {Skip}, take {Take})", data, skip, take);

            try
            {
                var (s, t) = Pagination.Normalizar(skip, take);
                return await _feedbackNPSRepository.ObterPorDataAsync(data, s, t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter feedbacks NPS da data {Data:yyyy-MM-dd}", data);
                throw;
            }
        }

        public async Task<FeedbackNPSEntity?> AdicionarAsync(FeedbackNPSRequestDto dto)
        {
            _logger.LogInformation("Registrando feedback NPS com nota {Nota}", dto.Nota);

            if (dto.IdResponsavel is null && dto.IdAnimal is null &&
                dto.IdClinica is null && dto.IdConsulta is null &&
                dto.IdVeterinario is null)
            {
                _logger.LogWarning("Feedback NPS sem nenhum contexto informado");
                throw new ArgumentException("Informe ao menos um contexto: responsável, animal, clínica, consulta ou veterinário.");
            }

            if (dto.IdResponsavel is not null && !await _feedbackNPSRepository.ResponsavelExisteAsync(dto.IdResponsavel.Value))
            {
                _logger.LogWarning("Responsável {IdResponsavel} não encontrado", dto.IdResponsavel);
                throw new ContextoNaoEncontradoException($"Responsável com ID {dto.IdResponsavel} não encontrado.");
            }

            if (dto.IdAnimal is not null && !await _feedbackNPSRepository.AnimalExisteAsync(dto.IdAnimal.Value))
            {
                _logger.LogWarning("Animal {IdAnimal} não encontrado", dto.IdAnimal);
                throw new ContextoNaoEncontradoException($"Animal com ID {dto.IdAnimal} não encontrado.");
            }

            if (dto.IdClinica is not null && !await _feedbackNPSRepository.ClinicaExisteAsync(dto.IdClinica.Value))
            {
                _logger.LogWarning("Clínica {IdClinica} não encontrada", dto.IdClinica);
                throw new ContextoNaoEncontradoException($"Clínica com ID {dto.IdClinica} não encontrada.");
            }

            if (dto.IdConsulta is not null && !await _feedbackNPSRepository.ConsultaExisteAsync(dto.IdConsulta.Value))
            {
                _logger.LogWarning("Consulta {IdConsulta} não encontrada", dto.IdConsulta);
                throw new ContextoNaoEncontradoException($"Consulta com ID {dto.IdConsulta} não encontrada.");
            }

            if (dto.IdVeterinario is not null && !await _feedbackNPSRepository.VeterinarioExisteAsync(dto.IdVeterinario.Value))
            {
                _logger.LogWarning("Veterinário {IdVeterinario} não encontrado", dto.IdVeterinario);
                throw new ContextoNaoEncontradoException($"Veterinário com ID {dto.IdVeterinario} não encontrado.");
            }

            try
            {
                return await _feedbackNPSRepository.AdicionarAsync(dto.ToFeedbackNPSEntity());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar feedback NPS");
                throw;
            }
        }

        public async Task<FeedbackNPSEntity?> DeletarAsync(int id)
        {
            _logger.LogInformation("Removendo feedback NPS {Id}", id);

            try
            {
                return await _feedbackNPSRepository.DeletarAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover feedback NPS {Id}", id);
                throw;
            }
        }
    }
}
