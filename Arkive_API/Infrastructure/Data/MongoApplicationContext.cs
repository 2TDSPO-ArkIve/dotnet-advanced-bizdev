using Arkive_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arkive_API.Infrastructure.Data
{
    public class MongoApplicationContext : DbContext
    {
        public MongoApplicationContext(DbContextOptions<MongoApplicationContext> options) : base(options) { }

        public DbSet<AuditoriaEntity> Auditoria { get; set; }
    }
}
