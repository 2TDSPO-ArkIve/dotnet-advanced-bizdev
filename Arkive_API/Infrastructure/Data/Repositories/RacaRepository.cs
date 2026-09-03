using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data.Repositories
{
    public class RacaRepository : IRacaRepository
    {
        private readonly ApplicationContext _context;

        public RacaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RacaEntity>> ObterTodosAsync()
        {
            try
            {
                return await _context.Raca
                    .Include(x => x.Especie)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<RacaEntity>> ObterAtivosAsync()
        {
            try
            {
                return await _context.Raca
                    .Include(x => x.Especie)
                    .Where(x => x.StAtivo == "S")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<RacaEntity>> ObterInativosAsync()
        {
            try
            {
                return await _context.Raca
                    .Include(x => x.Especie)
                    .Where(x => x.StAtivo == "N")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RacaEntity?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _context.Raca
                    .Include(x => x.Especie)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<RacaEntity>> ObterPorEspecieAsync(int idEspecie)
        {
            try
            {
                return await _context.Raca
                    .Include(x => x.Especie)
                    .Where(x => x.StAtivo == "S" && x.IdEspecie == idEspecie)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RacaEntity?> AdicionarAsync(RacaEntity entity)
        {
            try
            {
                entity.StAtivo = "S";

                _context.Raca.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RacaEntity?> EditarAsync(int id, RacaEntity entity)
        {
            try
            {
                var raca = await _context.Raca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (raca is null)
                    return null;

                raca.Raca = entity.Raca;
                raca.IdEspecie = entity.IdEspecie;
                raca.Porte = entity.Porte;

                _context.Raca.Update(raca);
                await _context.SaveChangesAsync();

                return raca;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RacaEntity?> ReativarAsync(int id)
        {
            try
            {
                var raca = await _context.Raca
                    .Include(x => x.Especie)
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (raca is null)
                    return null;

                raca.StAtivo = "S";

                _context.Raca.Update(raca);
                await _context.SaveChangesAsync();

                return raca;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RacaEntity?> InativarAsync(int id)
        {
            try
            {
                var raca = await _context.Raca
                    .Include(x => x.Especie)
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (raca is null)
                    return null;

                raca.StAtivo = "N";

                _context.Raca.Update(raca);
                await _context.SaveChangesAsync();

                return raca;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
