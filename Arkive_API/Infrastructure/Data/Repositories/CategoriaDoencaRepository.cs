using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data.Repositories
{
    public class CategoriaDoencaRepository : ICategoriaDoencaRepository
    {
        private readonly ApplicationContext _context;

        public CategoriaDoencaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PageResultModel<IEnumerable<CategoriaDoencaEntity>>> ObterTodosAsync(int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.CategoriaDoenca;

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<CategoriaDoencaEntity>>
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

        public async Task<PageResultModel<IEnumerable<CategoriaDoencaEntity>>> ObterAtivosAsync(int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "S");

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<CategoriaDoencaEntity>>
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

        public async Task<PageResultModel<IEnumerable<CategoriaDoencaEntity>>> ObterInativosAsync(int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.CategoriaDoenca
                    .Where(x => x.StAtivo == "N");

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<CategoriaDoencaEntity>>
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
