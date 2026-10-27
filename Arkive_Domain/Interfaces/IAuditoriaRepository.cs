using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Domain.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task<PageResultModel<IEnumerable<AuditoriaEntity>>> ObterTodosAsync(int skip = 0, int take = 50);
        Task<AuditoriaEntity?> AdicionarAsync(AuditoriaEntity entity);
    }
}
