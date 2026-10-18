using Arkive_API.Application.Dtos;
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
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
            Assert.Equal(idDoenca, resultado.Value!.Id);
            Assert.Equal(doenca.Nome, resultado.Value.Nome);
            Assert.Equal(doenca.StAtivo, resultado.Value.StAtivo);
            Assert.Equal(new[] { "self", "put_doenca", "delete_doenca" }, resultado.Value.Links.Select(l => l.Rel));
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
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
        public async Task AdicionarAsync_QuandoCategoriaNaoInformada_DeveAdicionarDoenca()
        {
            // Arrange
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = null };
            var entity = dto.ToDoencaEntity();

            _doencaRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _doencaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.Nome, resultado.Value!.Nome);
            Assert.Null(resultado.Value.IdCategoria);

            _categoriaDoencaRepository.Verify(obj => obj.ObterPorIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_QuandoCategoriaExisteEAtiva_DeveAdicionarDoenca()
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
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.Nome, resultado.Value!.Nome);
            Assert.Equal(idCategoria, resultado.Value.IdCategoria);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_QuandoCategoriaNaoExiste_DeveRetornar404()
        {
            // Arrange
            int idCategoria = 999;
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = idCategoria };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var resultado = await _doencaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _doencaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task AdicionarAsync_QuandoCategoriaEstaInativa_DeveRetornar404()
        {
            // Arrange
            int idCategoria = 1;
            var categoria = new CategoriaDoencaEntity { Id = idCategoria, Nome = "Viral", StAtivo = "N" };
            var dto = new DoencaRequestDto { Nome = "Cinomose", IdCategoria = idCategoria };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync(categoria);

            // Act
            var resultado = await _doencaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _doencaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<DoencaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task EditarAsync_QuandoCategoriaValidaOuNula_DeveEditarDoenca()
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
            Assert.Equal(idDoenca, resultado.Value!.Id);
            Assert.Equal(dto.Nome, resultado.Value.Nome);
            Assert.Null(resultado.Value.IdCategoria);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task EditarAsync_QuandoCategoriaInvalida_DeveRetornar404()
        {
            // Arrange
            int idDoenca = 1;
            int idCategoria = 999;
            var dto = new DoencaRequestDto { Nome = "Cinomose Canina", IdCategoria = idCategoria };

            _categoriaDoencaRepository.Setup(obj => obj.ObterPorIdAsync(idCategoria)).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var resultado = await _doencaUseCase.EditarAsync(idDoenca, dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

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
            Assert.Equal(idDoenca, resultado.Value!.Id);
            Assert.Equal(doenca.Nome, resultado.Value.Nome);
            Assert.Equal("S", resultado.Value.StAtivo);
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
            Assert.Equal(idDoenca, resultado.Value!.Id);
            Assert.Equal(doenca.Nome, resultado.Value.Nome);
            Assert.Equal("N", resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterTodasAsync_QuandoListaVazia_DeveRetornar204()
        {
            // Arrange
            _doencaRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<DoencaEntity>> { Data = new List<DoencaEntity>() });

            // Act
            var resultado = await _doencaUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterTodasAsync_QuandoRepositorioFalha_DeveRetornar400SemVazarMensagem()
        {
            // Arrange
            _doencaRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("ORA-12541: detalhe interno"));

            // Act
            var resultado = await _doencaUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("ORA-12541", resultado.Error);
        }

        [Fact]
        [Trait("UseCase", "Doencas")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _doencaRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((DoencaEntity?)null);

            // Act
            var resultado = await _doencaUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }
    }
}
