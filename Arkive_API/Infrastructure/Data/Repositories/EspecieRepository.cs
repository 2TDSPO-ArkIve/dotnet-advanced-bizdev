using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Repositories
{
    public class EspecieRepository : IEspecieRepository
    {
        private readonly ApplicationContext _context;

        public EspecieRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EspecieEntity>> ObterTodosAsync()
        {
            try
            {
                return await _context.Especie.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<EspecieEntity>> ObterAtivosAsync()
        {
            try
            {
                return await _context.Especie
                    .Where(x => x.StAtivo == "S")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<EspecieEntity>> ObterInativosAsync()
        {
            try
            {
                return await _context.Especie
                    .Where(x => x.StAtivo == "N")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EspecieEntity?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _context.Especie
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EspecieEntity?> AdicionarAsync(EspecieEntity entity)
        {
            try
            {
                entity.StAtivo = "S";

                _context.Especie.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EspecieEntity?> EditarAsync(int id, EspecieEntity entity)
        {
            try
            {
                var especie = await _context.Especie
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (especie is null)
                    return null;

                especie.Especie = entity.Especie;

                _context.Especie.Update(especie);
                await _context.SaveChangesAsync();

                return especie;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EspecieEntity?> ReativarAsync(int id)
        {
            try
            {
                var especie = await _context.Especie
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (especie is null)
                    return null;

                especie.StAtivo = "S";

                _context.Especie.Update(especie);
                await _context.SaveChangesAsync();

                return especie;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EspecieEntity?> InativarAsync(int id)
        {
            try
            {
                var especie = await _context.Especie
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (especie is null)
                    return null;

                especie.StAtivo = "N";

                _context.Especie.Update(especie);
                await _context.SaveChangesAsync();

                return especie;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
