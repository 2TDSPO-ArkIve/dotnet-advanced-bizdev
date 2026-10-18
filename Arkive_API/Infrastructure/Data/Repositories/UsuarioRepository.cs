using Arkive_API.Domain.Entities.External;
using Arkive_API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationContext _context;

        public UsuarioRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UsuarioExternal?> ObterPorLoginAsync(string login)
        {
            try
            {
                return await _context.Usuario
                    .FirstOrDefaultAsync(x => x.Login == login);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
