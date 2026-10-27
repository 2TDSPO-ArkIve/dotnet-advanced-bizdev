using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Arkive_Tests.App
{
    public class AuditoriaRepositoryTest
    {
        private readonly MongoApplicationContext _context;
        private readonly AuditoriaRepository _repository;

        public AuditoriaRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<MongoApplicationContext>()
                .UseInMemoryDatabase("AuditoriaTestDatabase")
                .Options;

            _context = new MongoApplicationContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _repository = new AuditoriaRepository(_context);
        }

        private static AuditoriaEntity Nova(string usuario, DateTime data) => new()
        {
            Id = ObjectId.GenerateNewId(),
            Usuario = usuario,
            Metodo = "POST",
            Rota = "/api/especies",
            StatusCode = 201,
            Data = data
        };

        [Fact]
        [Trait("Repository", "Auditoria")]
        public async Task AdicionarAsync_DevePersistirRegistro()
        {
            // Arrange
            var entity = Nova("admin", DateTime.UtcNow);

            // Act
            await _repository.AdicionarAsync(entity);

            // Assert
            Assert.Equal(1, await _context.Auditoria.CountAsync());
        }

        [Fact]
        [Trait("Repository", "Auditoria")]
        public async Task ObterTodosAsync_DeveOrdenarDoMaisRecenteEPaginar()
        {
            // Arrange
            var baseData = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _context.Auditoria.AddRange(Nova("antigo", baseData), Nova("meio", baseData.AddDays(1)), Nova("novo", baseData.AddDays(2)));
            _context.SaveChanges();

            // Act
            var resultado = await _repository.ObterTodosAsync(skip: 1, take: 2);

            // Assert
            Assert.Equal(3, resultado.TotalRegistros);
            Assert.Equal(new[] { "meio", "antigo" }, resultado.Data.Select(x => x.Usuario));
        }
    }
}
