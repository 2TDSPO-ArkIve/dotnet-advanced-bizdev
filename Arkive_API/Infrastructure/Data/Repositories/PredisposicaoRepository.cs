using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data.Repositories
{
    public class PredisposicaoRepository : IPredisposicaoRepository
    {
        private readonly ApplicationContext _context;

        public PredisposicaoRepository(ApplicationContext context)
        {
            _context = context;
        }

        private IQueryable<PredisposicaoEntity> ComRelacionamentos()
        {
            return _context.Predisposicao
                .Include(x => x.Especie)
                .Include(x => x.Raca)
                .Include(x => x.Doenca)
                    .ThenInclude(d => d.Categoria);
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterTodosAsync()
        {
            try
            {
                return await ComRelacionamentos().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PredisposicaoEntity?> ObterPorIdAsync(int id)
        {
            try
            {
                return await ComRelacionamentos().FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorEspecieAsync(int idEspecie)
        {
            try
            {
                return await ComRelacionamentos()
                    .Where(x => x.IdEspecie == idEspecie)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorRacaAsync(int idRaca)
        {
            try
            {
                return await ComRelacionamentos()
                    .Where(x => x.IdRaca == idRaca)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<PredisposicaoEntity>> ObterPorDoencaAsync(int idDoenca)
        {
            try
            {
                return await ComRelacionamentos()
                    .Where(x => x.IdDoenca == idDoenca)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PredisposicaoEntity?> AdicionarAsync(PredisposicaoEntity entity)
        {
            try
            {
                _context.Predisposicao.Add(entity);
                await _context.SaveChangesAsync();

                // Recarrega com os relacionamentos para devolver o objeto completo, igual ao Controller original
                return await ComRelacionamentos().FirstOrDefaultAsync(x => x.Id == entity.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PredisposicaoEntity?> DeletarAsync(int id)
        {
            try
            {
                var predisposicao = await ComRelacionamentos().FirstOrDefaultAsync(x => x.Id == id);

                if (predisposicao is null)
                    return null;

                _context.Predisposicao.Remove(predisposicao);
                await _context.SaveChangesAsync();

                return predisposicao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
