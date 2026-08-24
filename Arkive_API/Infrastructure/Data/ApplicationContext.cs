using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Entities.External;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<CategoriaDoencaEntity> CategoriaDoenca { get; set; }
        public DbSet<DoencaEntity> Doenca { get; set; }
        public DbSet<EspecieEntity> Especie { get; set; }
        public DbSet<FeedbackNPSEntity> FeedbackNPS { get; set; }
        public DbSet<PredisposicaoEntity> Predisposicao { get; set; }
        public DbSet<RacaEntity> Raca { get; set; }

        // Classes externas trabalhadas na API Java, usadas para consulta/validação
        public DbSet<ResponsavelExternal> Responsavel { get; set; }
        public DbSet<AnimalExternal> Animal { get; set; }
        public DbSet<ClinicaExternal> Clinica { get; set; }
        public DbSet<ConsultaExternal> Consulta { get; set; }
        public DbSet<VeterinarioExternal> Veterinario { get; set; }
    }
}
