using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Domain.Interfaces
{
    public interface IFeedbackNPSRepository
    {
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<FeedbackNPSEntity?> ObterPorIdAsync(int id);
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorNotaAsync(int nota, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorResponsavelAsync(int idResponsavel, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorAnimalAsync(int idAnimal, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorClinicaAsync(int idClinica, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorVeterinarioAsync(int idVeterinario, int skip = 0, int take = 50);
        Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorDataAsync(DateTime data, int skip = 0, int take = 50);
        Task<FeedbackNPSEntity?> AdicionarAsync(FeedbackNPSEntity entity);
        Task<FeedbackNPSEntity?> DeletarAsync(int id);

        // Verificações de existência nas tabelas externas (sincronizadas pela API Java).
        // Ficam aqui porque são consultas simples no mesmo ApplicationContext, não uma chamada
        // a outro serviço — continua sendo "acesso a dado", só que em tabela de outro domínio.
        Task<bool> ResponsavelExisteAsync(int id);
        Task<bool> AnimalExisteAsync(int id);
        Task<bool> ClinicaExisteAsync(int id);
        Task<bool> ConsultaExisteAsync(int id);
        Task<bool> VeterinarioExisteAsync(int id);
    }
}
