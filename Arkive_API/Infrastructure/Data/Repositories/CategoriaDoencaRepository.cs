using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Repositories
{
    public class CategoriaDoencaRepository : ICategoriaDoencaRepository
    {
        private readonly ApplicationContext _context;

        public CategoriaDoencaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterTodosAsync()
        {
            try
            {
                return await _context.CategoriaDoenca.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterAtivosAsync()
        {
            try
            {
                return await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "S")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CategoriaDoencaEntity>> ObterInativosAsync()
        {
            try
            {
                return await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "N")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoriaDoencaEntity?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _context.CategoriaDoenca
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoriaDoencaEntity?> AdicionarAsync(CategoriaDoencaEntity entity)
        {
            try
            {
                entity.StAtivo = "S";

                _context.CategoriaDoenca.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoriaDoencaEntity?> EditarAsync(int id, CategoriaDoencaEntity entity)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return null;

                categoria.Nome = entity.Nome;

                _context.CategoriaDoenca.Update(categoria);
                await _context.SaveChangesAsync();

                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoriaDoencaEntity?> ReativarAsync(int id)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return null;

                categoria.StAtivo = "S";

                _context.CategoriaDoenca.Update(categoria);
                await _context.SaveChangesAsync();

                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoriaDoencaEntity?> InativarAsync(int id)
        {
            try
            {
                var categoria = await _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (categoria is null)
                    return null;

                categoria.StAtivo = "N";

                _context.CategoriaDoenca.Update(categoria);
                await _context.SaveChangesAsync();

                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
