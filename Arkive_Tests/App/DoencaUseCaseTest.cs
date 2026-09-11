using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Mappers;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Arkive_Tests.App
{
    public class DoencaUseCaseTest
    {
        private readonly Mock<IDoencaRepository> _doencaRepository;
        private readonly Mock<ICategoriaDoencaRepository> _categoriaDoencaRepository;
        private readonly DoencaUseCase _doencaUseCase;

        public DoencaUseCaseTest()
        {
            _doencaRepository = new Mock<IDoencaRepository>();
            _categoriaDoencaRepository = new Mock<ICategoriaDoencaRepository>();

            // Isso é o que vamos testar
            _doencaUseCase = new DoencaUseCase(_doencaRepository.Object, _categoriaDoencaRepository.Object, NullLogger<DoencaUseCase>.Instance);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterTodasAsync_DeveRetornarTodasAsDoencas()
        {
            // Arrange
            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Id = 1, Nome = "Cinomose", StAtivo = "S" },
                new DoencaEntity { Id = 2, Nome = "Raiva", StAtivo = "N" }
            };

            _doencaRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<DoencaEntity>> { Data = doencas, TotalRegistros = doencas.Count });

            // Act
            var resultado = await _doencaUseCase.ObterTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Cinomose", item.Nome); Assert.Equal("S", item.StAtivo); },
                item => { Assert.Equal(2, item.Id); Assert.Equal("Raiva", item.Nome); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterAtivasAsync_DeveRetornarDoencasAtivas()
        {
            // Arrange
            var doencas = new List<DoencaEntity> { new DoencaEntity { Id = 1, Nome = "Cinomose", StAtivo = "S" } };

            _doencaRepository.Setup(obj => obj.ObterAtivosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<DoencaEntity>> { Data = doencas, TotalRegistros = doencas.Count });

            // Act
            var resultado = await _doencaUseCase.ObterAtivasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Cinomose", item.Nome); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterInativasAsync_DeveRetornarDoencasInativas()
        {
            // Arrange
            var doencas = new List<DoencaEntity> { new DoencaEntity { Id = 2, Nome = "Raiva", StAtivo = "N" } };

            _doencaRepository.Setup(obj => obj.ObterInativosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<DoencaEntity>> { Data = doencas, TotalRegistros = doencas.Count });

            // Act
            var resultado = await _doencaUseCase.ObterInativasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(2, item.Id); Assert.Equal("Raiva", item.Nome); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterPorIdAsync_DeveRetornarUmaDoenca()
        {
            // Arrange
            int idDoenca = 1;
            var doenca = new DoencaEntity { Id = idDoenca, Nome = "Cinomose", StAtivo = "S" };

            _doencaRepository.Setup(obj => obj.ObterPorIdAsync(idDoenca)).ReturnsAsync(doenca);

            // Act
            var resultado = await _doencaUseCase.ObterPorIdAsync(idDoenca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idDoenca, resultado!.Id);
            Assert.Equal(doenca.Nome, resultado.Nome);
            Assert.Equal(doenca.StAtivo, resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterPorNomeAsync_DeveRetornarDoencasFiltradasPorNome()
        {
            // Arrange
            var doencas = new List<DoencaEntity> { new DoencaEntity { Id = 1, Nome = "Cinomose", StAtivo = "S" } };

            _doencaRepository.Setup(obj => obj.ObterPorNomeAsync("cino")).ReturnsAsync(doencas);

            // Act
            var resultado = await _doencaUseCase.ObterPorNomeAsync("cino");

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Cinomose", item.Nome); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterPorCategoriaAsync_DeveRetornarDoencasDaCategoria()
        {
            // Arrange
            int idCategoria = 1;
            var doencas = new List<DoencaEntity> { new DoencaEntity { Id = 1, Nome = "Cinomose", IdCategoria = idCategoria, StAtivo = "S" } };

            _doencaRepository.Setup(obj => obj.ObterPorCategoriaAsync(idCategoria)).ReturnsAsync(doencas);

            // Act
            var resultado = await _doencaUseCase.ObterPorCategoriaAsync(idCategoria);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("Cinomose", item.Nome);
                    Assert.Equal(idCategoria, item.IdCategoria);
                    Assert.Equal("S", item.StAtivo);
                });
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_DeveAdicionarDoenca_QuandoCategoriaNaoInformada()
        {
            // Arrange
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = null };
            var entity = dto.ToDoencaEntity();

            _doencaRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _doencaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Nome, resultado!.Nome);
            Assert.Null(resultado.IdCategoria);

            _categoriaDoencaRepository.Verify(obj => obj.ObterPorIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_DeveAdicionarDoenca_QuandoCategoriaExisteEAtiva()
        {
            // Arrange
            int idCategoria = 1;
            var categoria = new CategoriaDoencaEntity { Id = idCategoria, Nome = "Viral", StAtivo = "S" };
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = idCategoria };
            var entity = dto.ToDoencaEntity();

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync(categoria);
            _doencaRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _doencaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Nome, resultado!.Nome);
            Assert.Equal(idCategoria, resultado.IdCategoria);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoCategoriaNaoExiste()
        {
            // Arrange
            int idCategoria = 999;
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = idCategoria };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<CategoriaNaoEncontradaException>(() => _doencaUseCase.AdicionarAsync(dto));

            _doencaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoCategoriaEstaInativa()
        {
            // Arrange
            int idCategoria = 1;
            var categoria = new CategoriaDoencaEntity { Id = idCategoria, Nome = "Viral", StAtivo = "N" };
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = idCategoria };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync(categoria);

            // Act & Assert
            await Assert.ThrowsAsync<CategoriaNaoEncontradaException>(() => _doencaUseCase.AdicionarAsync(dto));

            _doencaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task EditarAsync_DeveEditarDoenca_QuandoCategoriaValidaOuNula()
        {
            // Arrange
            int idDoenca = 1;
            var dto = new DoencaRequestDto { Nome = "Cinomose Canina", IdCategoria = null };
            var entity = dto.ToDoencaEntity();
            entity.Id = idDoenca;

            _doencaRepository.Setup(obj => obj.EditarAsync(idDoenca, It.IsAny<DoencaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _doencaUseCase.EditarAsync(idDoenca, dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idDoenca, resultado!.Id);
            Assert.Equal(dto.Nome, resultado.Nome);
            Assert.Null(resultado.IdCategoria);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task EditarAsync_DeveLancarExcecao_QuandoCategoriaInvalida()
        {
            // Arrange
            int idDoenca = 1;
            int idCategoria = 999;
            var dto = new DoencaRequestDto { Nome = "Cinomose Canina", IdCategoria = idCategoria };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<CategoriaNaoEncontradaException>(() => _doencaUseCase.EditarAsync(idDoenca, dto));

            _doencaRepository.Verify(obj => obj.EditarAsync(It.IsAny<int>(), It.IsAny<DoencaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ReativarAsync_DeveReativarDoenca()
        {
            // Arrange
            int idDoenca = 1;
            var doenca = new DoencaEntity { Id = idDoenca, Nome = "Cinomose", StAtivo = "S" };

            _doencaRepository.Setup(obj => obj.ReativarAsync(idDoenca)).ReturnsAsync(doenca);

            // Act
            var resultado = await _doencaUseCase.ReativarAsync(idDoenca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idDoenca, resultado!.Id);
            Assert.Equal(doenca.Nome, resultado.Nome);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task InativarAsync_DeveInativarDoenca()
        {
            // Arrange
            int idDoenca = 1;
            var doenca = new DoencaEntity { Id = idDoenca, Nome = "Cinomose", StAtivo = "N" };

            _doencaRepository.Setup(obj => obj.InativarAsync(idDoenca)).ReturnsAsync(doenca);

            // Act
            var resultado = await _doencaUseCase.InativarAsync(idDoenca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idDoenca, resultado!.Id);
            Assert.Equal(doenca.Nome, resultado.Nome);
            Assert.Equal("N", resultado.StAtivo);
        }
    }
}
