using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class DoencaRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly DoencaRepository _doencaRepository;

        public DoencaRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _doencaRepository = new DoencaRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterTodosAsync_DeveRetornarTodasAsDoencas()
        {
            // Arrange
            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Nome = "Cinomose", StAtivo = "S" },
                new DoencaEntity { Nome = "Raiva", StAtivo = "N" },
                new DoencaEntity { Nome = "Parvovirose", StAtivo = "S" }
            };

            _applicationContext.Doenca.AddRange(doencas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Cinomose", item.Nome); Assert.Equal("S", item.StAtivo); },
                item => { Assert.Equal("Raiva", item.Nome); Assert.Equal("N", item.StAtivo); },
                item => { Assert.Equal("Parvovirose", item.Nome); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterAtivosAsync_DeveRetornarSomenteDoencasAtivas()
        {
            // Arrange
            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Nome = "Cinomose", StAtivo = "S" },
                new DoencaEntity { Nome = "Raiva", StAtivo = "N" }
            };

            _applicationContext.Doenca.AddRange(doencas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ObterAtivosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Cinomose", item.Nome); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterInativosAsync_DeveRetornarSomenteDoencasInativas()
        {
            // Arrange
            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Nome = "Cinomose", StAtivo = "S" },
                new DoencaEntity { Nome = "Raiva", StAtivo = "N" }
            };

            _applicationContext.Doenca.AddRange(doencas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ObterInativosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Raiva", item.Nome); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterPorIdAsync_DeveRetornarDoenca_QuandoExiste()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "S" };

            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ObterPorIdAsync(doenca.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(doenca.Id, resultado!.Id);
            Assert.Equal(doenca.Nome, resultado.Nome);
            Assert.Equal(doenca.StAtivo, resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Act
            var resultado = await _doencaRepository.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterPorNomeAsync_DeveRetornarDoencasAtivasQueContenhamNome()
        {
            // Arrange
            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Nome = "Cinomose Canina", StAtivo = "S" },
                new DoencaEntity { Nome = "Raiva", StAtivo = "S" },
                new DoencaEntity { Nome = "Cinomose Felina", StAtivo = "N" }
            };

            _applicationContext.Doenca.AddRange(doencas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ObterPorNomeAsync("cinomose");

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal("Cinomose Canina", item.Nome); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ObterPorCategoriaAsync_DeveRetornarDoencasAtivasDaCategoria()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Nome = "Cinomose", StAtivo = "S", IdCategoria = categoria.Id },
                new DoencaEntity { Nome = "Raiva", StAtivo = "N", IdCategoria = categoria.Id },
                new DoencaEntity { Nome = "Outra", StAtivo = "S", IdCategoria = null }
            };

            _applicationContext.Doenca.AddRange(doencas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ObterPorCategoriaAsync(categoria.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("Cinomose", item.Nome);
                    Assert.Equal("S", item.StAtivo);
                    Assert.Equal(categoria.Id, item.IdCategoria);

                    // Garante que o relacionamento com Categoria foi carregado (Include)
                    Assert.NotNull(item.Categoria);
                    Assert.Equal(categoria.Id, item.Categoria!.Id);
                    Assert.Equal("Viral", item.Categoria.Nome);
                    Assert.Equal("S", item.Categoria.StAtivo);
                });
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task AdicionarAsync_DeveAdicionarDoencaComStAtivoS()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "N" };

            // Act
            var resultado = await _doencaRepository.AdicionarAsync(doenca);

            // Assert
            Assert.NotNull(resultado);

            var doencaNoDb = _applicationContext.Doenca.FirstOrDefault(x => x.Id == resultado!.Id);

            Assert.NotNull(doencaNoDb);
            Assert.Equal("Cinomose", doencaNoDb!.Nome);
            Assert.Equal("S", doencaNoDb.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task EditarAsync_DeveEditarDoencaAtivaExistente()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "S", CID = "A00" };
            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            var doencaEditada = new DoencaEntity { Nome = "Cinomose Canina", CID = "A01" };

            // Act
            var resultado = await _doencaRepository.EditarAsync(doenca.Id, doencaEditada);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(doenca.Id, resultado!.Id);
            Assert.Equal("Cinomose Canina", resultado.Nome);
            Assert.Equal("A01", resultado.CID);
            Assert.Null(resultado.IdCategoria);
            Assert.Null(resultado.Descricao);
            Assert.Null(resultado.Sintomas);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task EditarAsync_DeveRetornarNull_QuandoIdNaoExiste()
        {
            // Act
            var resultado = await _doencaRepository.EditarAsync(999, new DoencaEntity { Nome = "Outra" });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task EditarAsync_DeveRetornarNull_QuandoDoencaNaoExisteOuInativa()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "N" };
            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.EditarAsync(doenca.Id, new DoencaEntity { Nome = "Outra" });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ReativarAsync_DeveReativarDoencaInativa()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "N" };
            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ReativarAsync(doenca.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(doenca.Id, resultado!.Id);
            Assert.Equal(doenca.Nome, resultado.Nome);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task ReativarAsync_DeveRetornarNull_QuandoDoencaJaAtiva()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "S" };
            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.ReativarAsync(doenca.Id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task InativarAsync_DeveInativarDoencaAtiva()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "S" };
            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.InativarAsync(doenca.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(doenca.Id, resultado!.Id);
            Assert.Equal(doenca.Nome, resultado.Nome);
            Assert.Equal("N", resultado.StAtivo);
        }

        [Fact]
        [Trait("Repository", "Doencas")]
        public async Task InativarAsync_DeveRetornarNull_QuandoDoencaJaInativa()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = "Cinomose", StAtivo = "N" };
            _applicationContext.Doenca.Add(doenca);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _doencaRepository.InativarAsync(doenca.Id);

            // Assert
            Assert.Null(resultado);
        }
    }
}