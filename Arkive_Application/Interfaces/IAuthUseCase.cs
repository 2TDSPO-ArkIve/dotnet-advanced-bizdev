using Arkive_Application.Dtos;
using Arkive_Domain.Models;

namespace Arkive_Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<OperationResult<GetAuthDto?>> AutenticarAsync(PostAuthDto dto);
    }
}
