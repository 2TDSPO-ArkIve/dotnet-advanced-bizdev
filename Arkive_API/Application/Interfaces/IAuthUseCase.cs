using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<OperationResult<GetAuthDto?>> AutenticarAsync(PostAuthDto dto);
    }
}
