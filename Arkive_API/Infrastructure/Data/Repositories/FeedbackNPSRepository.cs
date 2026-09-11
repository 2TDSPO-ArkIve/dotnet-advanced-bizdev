using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data.Repositories
{
    public class FeedbackNPSRepository : IFeedbackNPSRepository
    {
        private readonly ApplicationContext _context;

        public FeedbackNPSRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterTodosAsync(int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.AsQueryable();

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<FeedbackNPSEntity?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _context.FeedbackNPS
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorNotaAsync(int nota, int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.Where(x => x.Nota == nota);

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorResponsavelAsync(int idResponsavel, int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.Where(x => x.IdResponsavel == idResponsavel);

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorAnimalAsync(int idAnimal, int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.Where(x => x.IdAnimal == idAnimal);

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorClinicaAsync(int idClinica, int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.Where(x => x.IdClinica == idClinica);

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorVeterinarioAsync(int idVeterinario, int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.Where(x => x.IdVeterinario == idVeterinario);

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<PageResultModel<IEnumerable<FeedbackNPSEntity>>> ObterPorDataAsync(DateTime data, int skip = 0, int take = 50)
        {
            try
            {
                var query = _context.FeedbackNPS.Where(x => x.DataFeedback.Date == data.Date);

                var total = await query.CountAsync();
                var dados = await query
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                return new PageResultModel<IEnumerable<FeedbackNPSEntity>>
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

        public async Task<FeedbackNPSEntity?> AdicionarAsync(FeedbackNPSEntity entity)
        {
            try
            {
                _context.FeedbackNPS.Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<FeedbackNPSEntity?> DeletarAsync(int id)
        {
            try
            {
                var feedback = await _context.FeedbackNPS
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (feedback is null)
                    return null;

                _context.FeedbackNPS.Remove(feedback);
                await _context.SaveChangesAsync();

                return feedback;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ResponsavelExisteAsync(int id)
        {
            return await _context.Responsavel.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> AnimalExisteAsync(int id)
        {
            return await _context.Animal.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ClinicaExisteAsync(int id)
        {
            return await _context.Clinica.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ConsultaExisteAsync(int id)
        {
            return await _context.Consulta.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VeterinarioExisteAsync(int id)
        {
            return await _context.Veterinario.AnyAsync(x => x.Id == id);
        }
    }
}
