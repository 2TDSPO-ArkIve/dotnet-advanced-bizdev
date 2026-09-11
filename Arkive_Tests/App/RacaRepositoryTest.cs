using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class RacaRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly RacaRepository _racaRepository;

        public RacaRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "RacaTestDatabase")
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _racaRepository = new RacaRepository(_applicationContext);

            // Semeia as entidades relacionadas (IdEspecie é FK obrigatória em RacaEntity).
            // Sem isso, o Include(Especie) do repositório não encontra a entidade pai
            // e a Raca some do resultado (relacionamento obrigatório -> join equivalente a INNER JOIN).
            SemearDadosRelacionados();
        }

        private void SemearDadosRelacionados()
        {
            _applicationContext.Especie.AddRange(
                new EspecieEntity { Id = 1, Especie = "Canina" },
                new EspecieEntity { Id = 2, Especie = "Felina" }
            );

            _applicationContext.SaveChanges();
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ObterTodosAsync_DeveRetornarTodasAsRacas()
        {
            // Arrange
            var racas = new List<RacaEntity>
            {
                new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" },
                new RacaEntity { Raca = "Siamês", IdEspecie = 2, StAtivo = "N" }
            };

            _applicationContext.Raca.AddRange(racas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("Siamês", item.Raca);
                    Assert.Equal(2, item.IdEspecie);
                    Assert.Equal("N", item.StAtivo);
                },
                item =>
                {
                    Assert.Equal("Labrador", item.Raca);
                    Assert.Equal(1, item.IdEspecie);
                    Assert.Equal("S", item.StAtivo);
                });
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ObterAtivosAsync_DeveRetornarSomenteRacasAtivas()
        {
            // Arrange
            var racas = new List<RacaEntity>
            {
                new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" },
                new RacaEntity { Raca = "Siamês", IdEspecie = 2, StAtivo = "N" }
            };

            _applicationContext.Raca.AddRange(racas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ObterAtivosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("Labrador", item.Raca);
                    Assert.Equal(1, item.IdEspecie);
                    Assert.Equal("S", item.StAtivo);
                });
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ObterInativosAsync_DeveRetornarSomenteRacasInativas()
        {
            // Arrange
            var racas = new List<RacaEntity>
            {
                new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" },
                new RacaEntity { Raca = "Siamês", IdEspecie = 2, StAtivo = "N" }
            };

            _applicationContext.Raca.AddRange(racas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ObterInativosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("Siamês", item.Raca);
                    Assert.Equal(2, item.IdEspecie);
                    Assert.Equal("N", item.StAtivo);
                });
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ObterPorIdAsync_QuandoExiste_DeveRetornarRaca()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" };

            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ObterPorIdAsync(raca.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(raca.Id, resultado!.Id);
            Assert.Equal(raca.Raca, resultado.Raca);
            Assert.Equal(raca.IdEspecie, resultado.IdEspecie);
            Assert.Equal(raca.StAtivo, resultado.StAtivo);

            // Garante que o relacionamento com Especie foi carregado (Include)
            Assert.NotNull(resultado.Especie);
            Assert.Equal("Canina", resultado.Especie!.Especie);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            // Act
            var resultado = await _racaRepository.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ObterPorEspecieAsync_DeveRetornarRacasAtivasDaEspecie()
        {
            // Arrange
            var racas = new List<RacaEntity>
            {
                new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" },
                new RacaEntity { Raca = "Poodle", IdEspecie = 1, StAtivo = "N" },
                new RacaEntity { Raca = "Siamês", IdEspecie = 2, StAtivo = "S" }
            };

            _applicationContext.Raca.AddRange(racas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ObterPorEspecieAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("Labrador", item.Raca);
                    Assert.Equal(1, item.IdEspecie);
                    Assert.Equal("S", item.StAtivo);
                });
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task AdicionarAsync_DeveAdicionarRacaComStAtivoS()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, Porte = "GRANDE", StAtivo = "N" };

            // Act
            var resultado = await _racaRepository.AdicionarAsync(raca);

            // Assert
            Assert.NotNull(resultado);

            var racaNoDb = _applicationContext.Raca.FirstOrDefault(x => x.Id == resultado!.Id);

            Assert.NotNull(racaNoDb);
            Assert.Equal("Labrador", racaNoDb!.Raca);
            Assert.Equal(1, racaNoDb.IdEspecie);
            Assert.Equal("GRANDE", racaNoDb.Porte);
            Assert.Equal("S", racaNoDb.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task EditarAsync_DeveEditarRacaAtivaExistente()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" };
            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.EditarAsync(raca.Id, new RacaEntity { Raca = "Labrador Retriever", IdEspecie = 1, Porte = "GRANDE" });

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(raca.Id, resultado!.Id);
            Assert.Equal("Labrador Retriever", resultado.Raca);
            Assert.Equal(1, resultado.IdEspecie);
            Assert.Equal("GRANDE", resultado.Porte);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task EditarAsync_QuandoIdNaoExiste_DeveRetornarNull()
        {
            // Act
            var resultado = await _racaRepository.EditarAsync(999, new RacaEntity { Raca = "Outra", IdEspecie = 1 });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task EditarAsync_QuandoRacaNaoExisteOuInativa_DeveRetornarNull()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "N" };
            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.EditarAsync(raca.Id, new RacaEntity { Raca = "Outra", IdEspecie = 1 });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ReativarAsync_DeveReativarRacaInativa()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "N" };
            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ReativarAsync(raca.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(raca.Id, resultado!.Id);
            Assert.Equal(raca.Raca, resultado.Raca);
            Assert.Equal(raca.IdEspecie, resultado.IdEspecie);
            Assert.Equal("S", resultado.StAtivo);
            Assert.NotNull(resultado.Especie);
            Assert.Equal("Canina", resultado.Especie!.Especie);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task ReativarAsync_QuandoRacaJaAtiva_DeveRetornarNull()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" };
            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.ReativarAsync(raca.Id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task InativarAsync_DeveInativarRacaAtiva()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "S" };
            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.InativarAsync(raca.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(raca.Id, resultado!.Id);
            Assert.Equal(raca.Raca, resultado.Raca);
            Assert.Equal(raca.IdEspecie, resultado.IdEspecie);
            Assert.Equal("N", resultado.StAtivo);
            Assert.NotNull(resultado.Especie);
            Assert.Equal("Canina", resultado.Especie!.Especie);
        }

        [Fact]
        [Trait("Repository", "Racas")]
        public async Task InativarAsync_QuandoRacaJaInativa_DeveRetornarNull()
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Labrador", IdEspecie = 1, StAtivo = "N" };
            _applicationContext.Raca.Add(raca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _racaRepository.InativarAsync(raca.Id);

            // Assert
            Assert.Null(resultado);
        }
    }
}