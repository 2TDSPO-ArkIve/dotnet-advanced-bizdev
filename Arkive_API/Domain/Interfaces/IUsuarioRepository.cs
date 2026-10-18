using Arkive_API.Domain.Entities.External;

namespace Arkive_API.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<UsuarioExternal?> ObterPorLoginAsync(string login);
    }
}
