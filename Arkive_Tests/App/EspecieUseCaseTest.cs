using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Arkive_Tests.App
{
    public class EspecieUseCaseTest
    {
        private readonly Mock<IEspecieRepository> _especieRepository;
        private readonly EspecieUseCase _especieUseCase;

        public EspecieUseCaseTest()
        {
            _especieRepository = new Mock<IEspecieRepository>();

            // Isso é o que vamos testar
            _especieUseCase = new EspecieUseCase(_especieRepository.Object, NullLogger<EspecieUseCase>.Instance);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterTodasAsync_DeveRetornarTodasAsEspecies()
        {
            // Arrange
            var especies = new List<EspecieEntity>
            {
                new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "N" }
            };

            _especieRepository.Setup(obj => obj.ObterTodosAsync()).ReturnsAsync(especies);

            // Act
            var resultado = await _especieUseCase.ObterTodasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Canina", item.Especie); Assert.Equal("S", item.StAtivo); },
                item => { Assert.Equal(2, item.Id); Assert.Equal("Felina", item.Especie); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterAtivasAsync_DeveRetornarEspeciesAtivas()
        {
            // Arrange
            var especies = new List<EspecieEntity> { new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" } };

            _especieRepository.Setup(obj => obj.ObterAtivosAsync()).ReturnsAsync(especies);

            // Act
            var resultado = await _especieUseCase.ObterAtivasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
                item => { Assert.Equal(1, item.Id); Assert.Equal("Canina", item.Especie); Assert.Equal("S", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterInativasAsync_DeveRetornarEspeciesInativas()
        {
            // Arrange
            var especies = new List<EspecieEntity> { new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "N" } };

            _especieRepository.Setup(obj => obj.ObterInativosAsync()).ReturnsAsync(especies);

            // Act
            var resultado = await _especieUseCase.ObterInativasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
                item => { Assert.Equal(2, item.Id); Assert.Equal("Felina", item.Especie); Assert.Equal("N", item.StAtivo); });
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterPorIdAsync_DeveRetornarUmaEspecie()
        {
            // Arrange
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "S" };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(idEspecie)).ReturnsAsync(especie);

            // Act
            var resultado = await _especieUseCase.ObterPorIdAsync(idEspecie);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idEspecie, resultado.Value!.Id);
            Assert.Equal(especie.Especie, resultado.Value.Especie);
            Assert.Equal(especie.StAtivo, resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _especieRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((EspecieEntity?)null);

            // Act
            var resultado = await _especieUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task AdicionarAsync_DeveAdicionarEspecie()
        {
            // Arrange
            var dto = new EspecieRequestDto { Especie = "Canina" };
            var entity = dto.ToEspecieEntity();

            _especieRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<EspecieEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _especieUseCase.AdicionarAsync(dto);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.Especie, resultado.Value!.Especie);
            Assert.Equal(entity.StAtivo, resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task EditarAsync_DeveEditarEspecieExistente()
        {
            // Arrange
            int idEspecie = 1;
            var dto = new EspecieRequestDto { Especie = "Canina Doméstica" };
            var entity = dto.ToEspecieEntity();
            entity.Id = idEspecie;

            _especieRepository.Setup(obj => obj.EditarAsync(idEspecie, It.IsAny<EspecieEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _especieUseCase.EditarAsync(idEspecie, dto);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idEspecie, resultado.Value!.Id);
            Assert.Equal(dto.Especie, resultado.Value.Especie);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task EditarAsync_QuandoEspecieNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new EspecieRequestDto { Especie = "Canina Doméstica" };

            _especieRepository.Setup(obj => obj.EditarAsync(It.IsAny<int>(), It.IsAny<EspecieEntity>())).ReturnsAsync((EspecieEntity?)null);

            // Act
            var resultado = await _especieUseCase.EditarAsync(999, dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ReativarAsync_DeveReativarEspecie()
        {
            // Arrange
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "S" };

            _especieRepository.Setup(obj => obj.ReativarAsync(idEspecie)).ReturnsAsync(especie);

            // Act
            var resultado = await _especieUseCase.ReativarAsync(idEspecie);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idEspecie, resultado.Value!.Id);
            Assert.Equal(especie.Especie, resultado.Value.Especie);
            Assert.Equal("S", resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task InativarAsync_DeveInativarEspecie()
        {
            // Arrange
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "N" };

            _especieRepository.Setup(obj => obj.InativarAsync(idEspecie)).ReturnsAsync(especie);

            // Act
            var resultado = await _especieUseCase.InativarAsync(idEspecie);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idEspecie, resultado.Value!.Id);
            Assert.Equal(especie.Especie, resultado.Value.Especie);
            Assert.Equal("N", resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterTodasAsync_QuandoListaVazia_DeveRetornar204()
        {
            // Arrange
            _especieRepository.Setup(obj => obj.ObterTodosAsync()).ReturnsAsync(new List<EspecieEntity>());

            // Act
            var resultado = await _especieUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Especies")]
        public async Task ObterTodasAsync_QuandoRepositorioFalha_DeveRetornar400SemVazarMensagem()
        {
            // Arrange
            _especieRepository.Setup(obj => obj.ObterTodosAsync())
                .ThrowsAsync(new Exception("ORA-12541: detalhe interno"));

            // Act
            var resultado = await _especieUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("ORA-12541", resultado.Error);
        }
    }
}
