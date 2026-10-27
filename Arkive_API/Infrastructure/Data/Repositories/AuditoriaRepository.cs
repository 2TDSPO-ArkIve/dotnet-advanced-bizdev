using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly MongoApplicationContext _context;

        public AuditoriaRepository(MongoApplicationContext context)
        {
            _context = context;
        }

        public async Task<PageResultModel<IEnumerable<AuditoriaEntity>>> ObterTodosAsync(int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.Auditoria;

                var total = await query.CountAsync();
                var dados = await query
                    .OrderByDescending(x => x.Data)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<AuditoriaEntity>>
                {
                    Data = dados,
                    Deslocamento = skip,
                    RegistroRetornado = take,
                    TotalRegistros = total
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<AuditoriaEntity?> AdicionarAsync(AuditoriaEntity entity)
        {
            try
            {
                await _context.Auditoria.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
