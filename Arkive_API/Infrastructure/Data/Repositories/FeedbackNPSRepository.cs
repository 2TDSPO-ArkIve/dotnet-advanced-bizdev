using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
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

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterTodosAsync()
        {
            try
            {
                return await _context.FeedbackNPS.ToListAsync();
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

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorNotaAsync(int nota)
        {
            try
            {
                return await _context.FeedbackNPS
                    .Where(x => x.Nota == nota)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorResponsavelAsync(int idResponsavel)
        {
            try
            {
                return await _context.FeedbackNPS
                    .Where(x => x.IdResponsavel == idResponsavel)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorAnimalAsync(int idAnimal)
        {
            try
            {
                return await _context.FeedbackNPS
                    .Where(x => x.IdAnimal == idAnimal)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorClinicaAsync(int idClinica)
        {
            try
            {
                return await _context.FeedbackNPS
                    .Where(x => x.IdClinica == idClinica)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorVeterinarioAsync(int idVeterinario)
        {
            try
            {
                return await _context.FeedbackNPS
                    .Where(x => x.IdVeterinario == idVeterinario)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<FeedbackNPSEntity>> ObterPorDataAsync(DateTime data)
        {
            try
            {
                return await _context.FeedbackNPS
                    .Where(x => x.DataFeedback.Date == data.Date)
                    .ToListAsync();
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
