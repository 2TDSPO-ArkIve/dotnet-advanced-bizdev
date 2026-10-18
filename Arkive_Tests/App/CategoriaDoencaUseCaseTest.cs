using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
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
            _categoriaDoencaUseCase = new CategoriaDoencaUseCase(_categoriaDoencaRepository.Object, NullLogger<CategoriaDoencaUseCase>.Instance);
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idCategoria, resultado.Value!.Id);
            Assert.Equal(categoria.Nome, resultado.Value.Nome);
            Assert.Equal(categoria.StAtivo, resultado.Value.StAtivo);
            Assert.Equal(new[] { "self", "put_categoria_doenca", "delete_categoria_doenca" }, resultado.Value.Links.Select(l => l.Rel));
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.Nome, resultado.Value!.Nome);
            Assert.Equal(entity.StAtivo, resultado.Value.StAtivo);
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idCategoria, resultado.Value!.Id);
            Assert.Equal(dto.Nome, resultado.Value.Nome);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task EditarAsync_QuandoCategoriaNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new CategoriaDoencaRequestDto { Nome = "Viral Respiratória" };

            _categoriaDoencaRepository.Setup(obj => obj.EditarAsync(It.IsAny<int>(), It.IsAny<CategoriaDoencaEntity>())).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var resultado = await _categoriaDoencaUseCase.EditarAsync(999, dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idCategoria, resultado.Value!.Id);
            Assert.Equal(categoria.Nome, resultado.Value.Nome);
            Assert.Equal("S", resultado.Value.StAtivo);
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idCategoria, resultado.Value!.Id);
            Assert.Equal(categoria.Nome, resultado.Value.Nome);
            Assert.Equal("N", resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterTodasAsync_QuandoListaVazia_DeveRetornar204()
        {
            // Arrange
            _categoriaDoencaRepository.Setup(obj => obj.ObterTodosAsync()).ReturnsAsync(new List<CategoriaDoencaEntity>());

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "CategoriaDoencas")]
        public async Task ObterTodasAsync_QuandoRepositorioFalha_DeveRetornar400SemVazarMensagem()
        {
            // Arrange
            _categoriaDoencaRepository.Setup(obj => obj.ObterTodosAsync())
                .ThrowsAsync(new Exception("ORA-12541: detalhe interno"));

            // Act
            var resultado = await _categoriaDoencaUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("ORA-12541", resultado.Error);
        }
    }
}
