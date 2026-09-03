using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class CategoriaDoencaRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly CategoriaDoencaRepository _categoriaDoencaRepository;

        public CategoriaDoencaRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _categoriaDoencaRepository = new CategoriaDoencaRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ObterTodosAsync_DeveRetornarTodasAsCategorias()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" },
                new CategoriaDoencaEntity { Nome = "Bacteriana", StAtivo = "N" },
                new CategoriaDoencaEntity { Nome = "Genética", StAtivo = "S" }
            };

            _applicationContext.CategoriaDoenca.AddRange(categorias);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ObterAtivosAsync_DeveRetornarSomenteCategoriasAtivas()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" },
                new CategoriaDoencaEntity { Nome = "Bacteriana", StAtivo = "N" }
            };

            _applicationContext.CategoriaDoenca.AddRange(categorias);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.ObterAtivosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Viral", resultado.First().Nome);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ObterInativosAsync_DeveRetornarSomenteCategoriasInativas()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" },
                new CategoriaDoencaEntity { Nome = "Bacteriana", StAtivo = "N" }
            };

            _applicationContext.CategoriaDoenca.AddRange(categorias);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.ObterInativosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Bacteriana", resultado.First().Nome);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ObterPorIdAsync_DeveRetornarCategoria_QuandoExiste()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" };

            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.ObterPorIdAsync(categoria.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(categoria.Id, resultado!.Id);
            Assert.Equal(categoria.Nome, resultado.Nome);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Act
            var resultado = await _categoriaDoencaRepository.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task AdicionarAsync_DeveAdicionarCategoriaComStAtivoS()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "N" };

            // Act
            var resultado = await _categoriaDoencaRepository.AdicionarAsync(categoria);

            // Assert
            Assert.NotNull(resultado);

            var categoriaNoDb = _applicationContext.CategoriaDoenca.FirstOrDefault(x => x.Id == resultado!.Id);

            Assert.NotNull(categoriaNoDb);
            Assert.Equal("Viral", categoriaNoDb!.Nome);
            Assert.Equal("S", categoriaNoDb.StAtivo);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task EditarAsync_DeveEditarCategoriaAtivaExistente()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.EditarAsync(categoria.Id, new CategoriaDoencaEntity { Nome = "Viral Respiratória" });

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Viral Respiratória", resultado!.Nome);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task EditarAsync_DeveRetornarNull_QuandoCategoriaNaoExisteOuInativa()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "N" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.EditarAsync(categoria.Id, new CategoriaDoencaEntity { Nome = "Outra" });

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ReativarAsync_DeveReativarCategoriaInativa()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "N" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.ReativarAsync(categoria.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("S", resultado!.StAtivo);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task ReativarAsync_DeveRetornarNull_QuandoCategoriaJaAtiva()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.ReativarAsync(categoria.Id);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task InativarAsync_DeveInativarCategoriaAtiva()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "S" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.InativarAsync(categoria.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("N", resultado!.StAtivo);
        }

        [Fact]
        [Trait("Repository", "CategoriaDoencas")]
        public async Task InativarAsync_DeveRetornarNull_QuandoCategoriaJaInativa()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Nome = "Viral", StAtivo = "N" };
            _applicationContext.CategoriaDoenca.Add(categoria);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _categoriaDoencaRepository.InativarAsync(categoria.Id);

            // Assert
            Assert.Null(resultado);
        }
    }
}
