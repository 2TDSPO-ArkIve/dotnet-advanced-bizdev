using Arkive_Domain.Entities.External;

namespace Arkive_Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<UsuarioExternal?> ObterPorLoginAsync(string login);
    }
}
