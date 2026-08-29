using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Repositories
{
    public class DoencaRepository : IDoencaRepository
    {
        private readonly ApplicationContext _context;

        public DoencaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoencaEntity>> ObterTodosAsync()
        {
            try
            {
                return await _context.Doenca
                    .Include(x => x.Categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DoencaEntity>> ObterAtivosAsync()
        {
            try
            {
                return await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DoencaEntity>> ObterInativosAsync()
        {
            try
            {
                return await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "N")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DoencaEntity?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _context.Doenca
                    .Include(x => x.Categoria)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DoencaEntity>> ObterPorNomeAsync(string nome)
        {
            try
            {
                return await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S" && x.Nome.ToLower().Contains(nome.ToLower()))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<DoencaEntity>> ObterPorCategoriaAsync(int idCategoria)
        {
            try
            {
                return await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S" && x.IdCategoria == idCategoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DoencaEntity?> AdicionarAsync(DoencaEntity entity)
        {
            try
            {
                entity.StAtivo = "S";

                _context.Doenca.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DoencaEntity?> EditarAsync(int id, DoencaEntity entity)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (doenca is null)
                    return null;

                doenca.Nome = entity.Nome;
                doenca.IdCategoria = entity.IdCategoria;
                doenca.Descricao = entity.Descricao;
                doenca.CID = entity.CID;
                doenca.Sintomas = entity.Sintomas;

                _context.Doenca.Update(doenca);
                await _context.SaveChangesAsync();

                return doenca;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DoencaEntity?> ReativarAsync(int id)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (doenca is null)
                    return null;

                doenca.StAtivo = "S";

                _context.Doenca.Update(doenca);
                await _context.SaveChangesAsync();

                return doenca;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DoencaEntity?> InativarAsync(int id)
        {
            try
            {
                var doenca = await _context.Doenca
                    .Include(x => x.Categoria)
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (doenca is null)
                    return null;

                doenca.StAtivo = "N";

                _context.Doenca.Update(doenca);
                await _context.SaveChangesAsync();

                return doenca;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
