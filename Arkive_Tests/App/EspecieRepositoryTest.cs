using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class EspecieRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly EspecieRepository _especieRepository;

        public EspecieRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "EspecieTestDatabase")
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _especieRepository = new EspecieRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ObterTodosAsync_DeveRetornarTodasAsEspecies()
        {
            // Arrange
            var especies = new List<EspecieEntity>
            {
                new EspecieEntity { Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Especie = "Felina", StAtivo = "N" },
                new EspecieEntity { Especie = "Equina", StAtivo = "S" }
            };

            _applicationContext.Especie.AddRange(especies);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Canina", item.Especie); Assert.Equal("S", item.StAtivo); },
                item => { Assert.Equal("Felina", item.Especie); Assert.Equal("N", item.StAtivo); },
                item => { Assert.Equal("Equina", item.Especie); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ObterAtivosAsync_DeveRetornarSomenteEspeciesAtivas()
        {
            // Arrange
            var especies = new List<EspecieEntity>
            {
                new EspecieEntity { Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Especie = "Felina", StAtivo = "N" }
            };

            _applicationContext.Especie.AddRange(especies);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.ObterAtivosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Canina", item.Especie); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ObterInativosAsync_DeveRetornarSomenteEspeciesInativas()
        {
            // Arrange
            var especies = new List<EspecieEntity>
            {
                new EspecieEntity { Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Especie = "Felina", StAtivo = "N" }
            };

            _applicationContext.Especie.AddRange(especies);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.ObterInativosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Felina", item.Especie); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ObterPorIdAsync_DeveRetornarEspecie_QuandoExiste()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "S" };

            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.ObterPorIdAsync(especie.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(especie.Id, resultado!.Id);
            Assert.Equal(especie.Especie, resultado.Especie);
            Assert.Equal(especie.StAtivo, resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Act
            var resultado = await _especieRepository.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task AdicionarAsync_DeveAdicionarEspecieComStAtivoS()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "N" };

            // Act
            var resultado = await _especieRepository.AdicionarAsync(especie);

            // Assert
            Assert.NotNull(resultado);

            var especieNoDb = _applicationContext.Especie.FirstOrDefault(x => x.Id == resultado!.Id);

            Assert.NotNull(especieNoDb);
            Assert.Equal("Canina", especieNoDb!.Especie);
            Assert.Equal("S", especieNoDb.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task EditarAsync_DeveEditarEspecieAtivaExistente()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "S" };
            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.EditarAsync(especie.Id, new EspecieEntity { Especie = "Canina Doméstica" });

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(especie.Id, resultado!.Id);
            Assert.Equal("Canina Doméstica", resultado.Especie);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task EditarAsync_DeveRetornarNull_QuandoIdNaoExiste()
        {
            // Act
            var resultado = await _especieRepository.EditarAsync(999, new EspecieEntity { Especie = "Outra" });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task EditarAsync_DeveRetornarNull_QuandoEspecieNaoExisteOuInativa()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "N" };
            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.EditarAsync(especie.Id, new EspecieEntity { Especie = "Outra" });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ReativarAsync_DeveReativarEspecieInativa()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "N" };
            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.ReativarAsync(especie.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(especie.Id, resultado!.Id);
            Assert.Equal(especie.Especie, resultado.Especie);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task ReativarAsync_DeveRetornarNull_QuandoEspecieJaAtiva()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "S" };
            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.ReativarAsync(especie.Id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task InativarAsync_DeveInativarEspecieAtiva()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "S" };
            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.InativarAsync(especie.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(especie.Id, resultado!.Id);
            Assert.Equal(especie.Especie, resultado.Especie);
            Assert.Equal("N", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Especies")]
        public async Task InativarAsync_DeveRetornarNull_QuandoEspecieJaInativa()
        {
            // Arrange
            var especie = new EspecieEntity { Especie = "Canina", StAtivo = "N" };
            _applicationContext.Especie.Add(especie);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _especieRepository.InativarAsync(especie.Id);

            // Assert
            Assert.Null(resultado);
        }
    }
}