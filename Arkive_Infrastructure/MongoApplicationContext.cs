using Arkive_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Infrastructure
{
    public class MongoApplicationContext : DbContext
    {
        public MongoApplicationContext(DbContextOptions<MongoApplicationContext> options) : base(options) { }

        public DbSet<AuditoriaEntity> Auditoria { get; set; }
    }
}
