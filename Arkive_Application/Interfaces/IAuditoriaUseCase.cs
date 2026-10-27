using Arkive_Application.Dtos;
using Arkive_Domain.Models;

namespace Arkive_Application.Interfaces
{
    public interface IAuditoriaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>> ObterTodasAsync(int skip = 0, int take = 50);
    }
}
