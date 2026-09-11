using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Mappers;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Arkive_Tests.App
{
    public class RacaUseCaseTest
    {
        private readonly Mock<IRacaRepository> _racaRepository;
        private readonly Mock<IEspecieRepository> _especieRepository;
        private readonly RacaUseCase _racaUseCase;

        public RacaUseCaseTest()
        {
            _racaRepository = new Mock<IRacaRepository>();
            _especieRepository = new Mock<IEspecieRepository>();

            // Isso é o que vamos testar
            _racaUseCase = new RacaUseCase(_racaRepository.Object, _especieRepository.Object, NullLogger<RacaUseCase>.Instance);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterTodasAsync_DeveRetornarTodasAsRacas()
        {
            // Arrange
            var racas = new List<RacaEntity>
            {
                new RacaEntity { Id = 1, Raca = "Labrador", StAtivo = "S" },
                new RacaEntity { Id = 2, Raca = "Siamês", StAtivo = "N" }
            };

            _racaRepository.Setup(obj => obj.ObterTodosAsync()).ReturnsAsync(racas);

            // Act
            var resultado = await _racaUseCase.ObterTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("Labrador", item.Raca);
                    Assert.Equal("S", item.StAtivo);
                },
                item =>
                {
                    Assert.Equal(2, item.Id);
                    Assert.Equal("Siamês", item.Raca);
                    Assert.Equal("N", item.StAtivo);
                });
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterAtivasAsync_DeveRetornarRacasAtivas()
        {
            // Arrange
            var racas = new List<RacaEntity> { new RacaEntity { Id = 1, Raca = "Labrador", StAtivo = "S" } };

            _racaRepository.Setup(obj => obj.ObterAtivosAsync()).ReturnsAsync(racas);

            // Act
            var resultado = await _racaUseCase.ObterAtivasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("Labrador", item.Raca);
                    Assert.Equal("S", item.StAtivo);
                });
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterInativasAsync_DeveRetornarRacasInativas()
        {
            // Arrange
            var racas = new List<RacaEntity> { new RacaEntity { Id = 2, Raca = "Siamês", StAtivo = "N" } };

            _racaRepository.Setup(obj => obj.ObterInativosAsync()).ReturnsAsync(racas);

            // Act
            var resultado = await _racaUseCase.ObterInativasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(2, item.Id);
                    Assert.Equal("Siamês", item.Raca);
                    Assert.Equal("N", item.StAtivo);
                });
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterPorIdAsync_DeveRetornarUmaRaca()
        {
            // Arrange
            int idRaca = 1;
            var raca = new RacaEntity { Id = idRaca, Raca = "Labrador", StAtivo = "S" };

            _racaRepository.Setup(obj => obj.ObterPorIdAsync(idRaca)).ReturnsAsync(raca);

            // Act
            var resultado = await _racaUseCase.ObterPorIdAsync(idRaca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idRaca, resultado!.Id);
            Assert.Equal(raca.Raca, resultado.Raca);
            Assert.Equal(raca.StAtivo, resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterPorEspecieAsync_DeveRetornarRacasDaEspecie()
        {
            // Arrange
            int idEspecie = 1;
            var racas = new List<RacaEntity> { new RacaEntity { Id = 1, Raca = "Labrador", IdEspecie = idEspecie, StAtivo = "S" } };

            _racaRepository.Setup(obj => obj.ObterPorEspecieAsync(idEspecie)).ReturnsAsync(racas);

            // Act
            var resultado = await _racaUseCase.ObterPorEspecieAsync(idEspecie);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal("Labrador", item.Raca);
                    Assert.Equal(idEspecie, item.IdEspecie);
                    Assert.Equal("S", item.StAtivo);
                });
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task AdicionarAsync_QuandoEspecieExisteEAtiva_DeveAdicionarRaca()
        {
            // Arrange
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "S" };
            var dto = new RacaRequestDto { Raca = "Labrador", IdEspecie = idEspecie };
            var entity = dto.ToRacaEntity();

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(idEspecie)).ReturnsAsync(especie);
            _racaRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<RacaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _racaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Raca, resultado!.Raca);
            Assert.Equal(dto.IdEspecie, resultado.IdEspecie);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task AdicionarAsync_QuandoEspecieNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            var dto = new RacaRequestDto { Raca = "Labrador", IdEspecie = 999 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((EspecieEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<EspecieNaoEncontradaException>(() => _racaUseCase.AdicionarAsync(dto));

            _racaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<RacaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task AdicionarAsync_QuandoEspecieEstaInativa_DeveLancarExcecao()
        {
            // Arrange
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "N" };
            var dto = new RacaRequestDto { Raca = "Labrador", IdEspecie = idEspecie };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(idEspecie)).ReturnsAsync(especie);

            // Act & Assert
            await Assert.ThrowsAsync<EspecieNaoEncontradaException>(() => _racaUseCase.AdicionarAsync(dto));

            _racaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<RacaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task EditarAsync_QuandoEspecieValida_DeveEditarRaca()
        {
            // Arrange
            int idRaca = 1;
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "S" };
            var dto = new RacaRequestDto { Raca = "Labrador Retriever", IdEspecie = idEspecie };
            var entity = dto.ToRacaEntity();
            entity.Id = idRaca;

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(idEspecie)).ReturnsAsync(especie);
            _racaRepository.Setup(obj => obj.EditarAsync(idRaca, It.IsAny<RacaEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _racaUseCase.EditarAsync(idRaca, dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idRaca, resultado!.Id);
            Assert.Equal(dto.Raca, resultado.Raca);
            Assert.Equal(dto.IdEspecie, resultado.IdEspecie);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task EditarAsync_QuandoEspecieInvalida_DeveLancarExcecao()
        {
            // Arrange
            int idRaca = 1;
            var dto = new RacaRequestDto { Raca = "Labrador Retriever", IdEspecie = 999 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((EspecieEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<EspecieNaoEncontradaException>(() => _racaUseCase.EditarAsync(idRaca, dto));

            _racaRepository.Verify(obj => obj.EditarAsync(It.IsAny<int>(), It.IsAny<RacaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ReativarAsync_DeveReativarRaca()
        {
            // Arrange
            int idRaca = 1;
            var raca = new RacaEntity { Id = idRaca, Raca = "Labrador", StAtivo = "S" };

            _racaRepository.Setup(obj => obj.ReativarAsync(idRaca)).ReturnsAsync(raca);

            // Act
            var resultado = await _racaUseCase.ReativarAsync(idRaca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idRaca, resultado!.Id);
            Assert.Equal(raca.Raca, resultado.Raca);
            Assert.Equal("S", resultado.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task InativarAsync_DeveInativarRaca()
        {
            // Arrange
            int idRaca = 1;
            var raca = new RacaEntity { Id = idRaca, Raca = "Labrador", StAtivo = "N" };

            _racaRepository.Setup(obj => obj.InativarAsync(idRaca)).ReturnsAsync(raca);

            // Act
            var resultado = await _racaUseCase.InativarAsync(idRaca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idRaca, resultado!.Id);
            Assert.Equal(raca.Raca, resultado.Raca);
            Assert.Equal("N", resultado.StAtivo);
        }
    }
}
