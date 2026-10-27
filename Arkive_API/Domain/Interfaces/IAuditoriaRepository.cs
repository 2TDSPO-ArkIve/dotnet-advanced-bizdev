using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Domain.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task<PageResultModel<IEnumerable<AuditoriaEntity>>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<AuditoriaEntity?> AdicionarAsync(AuditoriaEntity entity);
    }
}
