using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IAuditoriaUseCase
    {
        Task<OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>> ObterTodasAsync(int skip = 0, int take = 50);
    }
}
