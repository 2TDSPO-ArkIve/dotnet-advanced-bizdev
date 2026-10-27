using Arkive_Domain.Entities;
using Arkive_Domain.Interfaces;
using Arkive_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Infrastructure.Repositories
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

        // Campos aceitos em ordenarPor; qualquer outro valor cai em id asc
        private static IOrderedQueryable<PredisposicaoEntity> Ordenar(IQueryable<PredisposicaoEntity> query, string? ordenarPor, string? direcao)
        {
            var desc = Ordenacao.Desc(direcao);
            return ordenarPor?.ToLowerInvariant() switch
            {
                "id" => query.Por(x => x.Id, desc),
                _ => query.OrderBy(x => x.Id)
            };
        }

        public async Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterTodosAsync(int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = ComRelacionamentos();

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<PredisposicaoEntity>>
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

        public async Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorEspecieAsync(int idEspecie, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = ComRelacionamentos().Where(x => x.IdEspecie == idEspecie);

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<PredisposicaoEntity>>
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

        public async Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorRacaAsync(int idRaca, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = ComRelacionamentos().Where(x => x.IdRaca == idRaca);

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<PredisposicaoEntity>>
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

        public async Task<PageResultModel<IEnumerable<PredisposicaoEntity>>> ObterPorDoencaAsync(int idDoenca, int skip = 0, int take = 50, string? ordenarPor = null, string direcao = "asc")
        {
            try
            {
                var query = ComRelacionamentos().Where(x => x.IdDoenca == idDoenca);

                var total = await query.CountAsync();
                var dados = await Ordenar(query, ordenarPor, direcao)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<PredisposicaoEntity>>
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
