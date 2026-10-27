using Arkive_Domain.Entities;
using Arkive_Domain.Interfaces;
using Arkive_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Infrastructure.Repositories
{
    public class EspecieRepository : IEspecieRepository
    {
        private readonly ApplicationContext _context;

        public EspecieRepository(ApplicationContext context)
        {
            _context = context;
        }

        // Campos aceitos em ordenarPor; qualquer outro valor cai em id asc
        private static IOrderedQueryable<EspecieEntity> Ordenar(IQueryable<EspecieEntity> query, string? ordenarPor, string? direcao)
        {
            var desc = Ordenacao.Desc(direcao);
            return ordenarPor?.ToLowerInvariant() switch
            {
                "nome" => query.Por(x => x.Especie, desc).ThenBy(x => x.Id),
                "id" => query.Por(x => x.Id, desc),
                _ => query.OrderBy(x => x.Id)
            };
        }

        public async Task<PageResultModel<IEnumerable<EspecieEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = _context.Especie;

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<EspecieEntity>>
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

        public async Task<PageResultModel<IEnumerable<EspecieEntity>>> ObterAtivosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = _context.Especie
                    .Where(x => x.StAtivo == "S");

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<EspecieEntity>>
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

        public async Task<PageResultModel<IEnumerable<EspecieEntity>>> ObterInativosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = _context.Especie
                    .Where(x => x.StAtivo == "N");

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<EspecieEntity>>
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
