using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Moq;

namespace Arkive_Tests.App
{
    public class CategoriaDoencaUseCaseTest
    {
        private readonly Mock<ICategoriaDoencaRepository> _categoriaDoencaRepository;
        private readonly CategoriaDoencaUseCase _categoriaDoencaUseCase;

        public CategoriaDoencaUseCaseTest()
        {
            _categoriaDoencaRepository = new Mock<ICategoriaDoencaRepository>();

            // Isso é o que vamos testar
            _categoriaDoencaUseCase = new CategoriaDoencaUseCase(_categoriaDoencaRepository.Object);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterTodasAsync_DeveRetornarTodasAsCategorias()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Id = 1, Nome = "Viral", StAtivo = "S" },
                new CategoriaDoencaEntity { Id = 2, Nome = "Bacteriana", StAtivo = "N" }
            };

            _categoriaDoencaRepository.Setup(obj => obj.ObterTodosAsync()).ReturnsAsync(categorias);

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Viral", item.Nome); Assert.Equal("S", item.StAtivo); },
                item => { Assert.Equal(2, item.Id); Assert.Equal("Bacteriana", item.Nome); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterAtivasAsync_DeveRetornarCategoriasAtivas()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity> { new CategoriaDoencaEntity { Id = 1, Nome = "Viral", StAtivo = "S" } };

            _categoriaDoencaRepository.Setup(obj => obj.ObterAtivosAsync()).ReturnsAsync(categorias);

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterAtivasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Viral", item.Nome); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterInativasAsync_DeveRetornarCategoriasInativas()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity> { new CategoriaDoencaEntity { Id = 2, Nome = "Bacteriana", StAtivo = "N" } };

            _categoriaDoencaRepository.Setup(obj => obj.ObterInativosAsync()).ReturnsAsync(categorias);

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterInativasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal(2, item.Id); Assert.Equal("Bacteriana", item.Nome); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterPorIdAsync_DeveRetornarUmaCategoria()
        {
            // Arrange
            int idCategoria = 1;
            var categoria = new CategoriaDoencaEntity { Id = idCategoria, Nome = "Viral", StAtivo = "S" };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync(categoria);

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterPorIdAsync(idCategoria);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idCategoria, resultado!.Id);
            Assert.Equal(categoria.Nome, resultado.Nome);
            Assert.Equal(categoria.StAtivo, resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Arrange
            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task AdicionarAsync_DeveAdicionarCategoria()
        {
            // Arrange
            var dto = new CategoriaDoencaRequestDto { Nome = "Viral" };
            var entity = dto.ToCategoriaDoencaEntity();

            _categoriaDoencaRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<CategoriaDoencaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _categoriaDoencaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Nome, resultado!.Nome);
            Assert.Equal(entity.StAtivo, resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task EditarAsync_DeveEditarCategoriaExistente()
        {
            // Arrange
            int idCategoria = 1;
            var dto = new CategoriaDoencaRequestDto { Nome = "Viral Respiratória" };
            var entity = dto.ToCategoriaDoencaEntity();
            entity.Id = idCategoria;

            _categoriaDoencaRepository.Setup(obj => obj.EditarAsync(idCategoria, It.IsAny<CategoriaDoencaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _categoriaDoencaUseCase.EditarAsync(idCategoria, dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idCategoria, resultado!.Id);
            Assert.Equal(dto.Nome, resultado.Nome);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task EditarAsync_DeveRetornarNull_QuandoCategoriaNaoExiste()
        {
            // Arrange
            var dto = new CategoriaDoencaRequestDto { Nome = "Viral Respiratória" };

            _categoriaDoencaRepository.Setup(obj => obj.EditarAsync(It.IsAny<int>(), It.IsAny<CategoriaDoencaEntity>())).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var resultado = await _categoriaDoencaUseCase.EditarAsync(999, dto);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ReativarAsync_DeveReativarCategoria()
        {
            // Arrange
            int idCategoria = 1;
            var categoria = new CategoriaDoencaEntity { Id = idCategoria, Nome = "Viral", StAtivo = "S" };

            _categoriaDoencaRepository.Setup(obj => obj.ReativarAsync(idCategoria)).ReturnsAsync(categoria);

            // Act
            var resultado = await _categoriaDoencaUseCase.ReativarAsync(idCategoria);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idCategoria, resultado!.Id);
            Assert.Equal(categoria.Nome, resultado.Nome);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task InativarAsync_DeveInativarCategoria()
        {
            // Arrange
            int idCategoria = 1;
            var categoria = new CategoriaDoencaEntity { Id = idCategoria, Nome = "Viral", StAtivo = "N" };

            _categoriaDoencaRepository.Setup(obj => obj.InativarAsync(idCategoria)).ReturnsAsync(categoria);

            // Act
            var resultado = await _categoriaDoencaUseCase.InativarAsync(idCategoria);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idCategoria, resultado!.Id);
            Assert.Equal(categoria.Nome, resultado.Nome);
            Assert.Equal("N", resultado.StAtivo);
        }
    }
}
