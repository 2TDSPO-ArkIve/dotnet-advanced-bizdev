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

            _racaRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PageResultModel<IEnumerable<RacaEntity>> { Data = racas });

            // Act
            var resultado = await _racaUseCase.ObterTodasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
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

            _racaRepository.Setup(obj => obj.ObterAtivosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PageResultModel<IEnumerable<RacaEntity>> { Data = racas });

            // Act
            var resultado = await _racaUseCase.ObterAtivasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
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

            _racaRepository.Setup(obj => obj.ObterInativosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PageResultModel<IEnumerable<RacaEntity>> { Data = racas });

            // Act
            var resultado = await _racaUseCase.ObterInativasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idRaca, resultado.Value!.Id);
            Assert.Equal(raca.Raca, resultado.Value.Raca);
            Assert.Equal(raca.StAtivo, resultado.Value.StAtivo);
            Assert.Equal(new[] { "self", "put_raca", "delete_raca" }, resultado.Value.Links.Select(l => l.Rel));
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
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!,
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.Raca, resultado.Value!.Raca);
            Assert.Equal(dto.IdEspecie, resultado.Value.IdEspecie);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task AdicionarAsync_QuandoEspecieNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new RacaRequestDto { Raca = "Labrador", IdEspecie = 999 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((EspecieEntity?)null);

            // Act
            var resultado = await _racaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _racaRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<RacaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task AdicionarAsync_QuandoEspecieEstaInativa_DeveRetornar404()
        {
            // Arrange
            int idEspecie = 1;
            var especie = new EspecieEntity { Id = idEspecie, Especie = "Canina", StAtivo = "N" };
            var dto = new RacaRequestDto { Raca = "Labrador", IdEspecie = idEspecie };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(idEspecie)).ReturnsAsync(especie);

            // Act
            var resultado = await _racaUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idRaca, resultado.Value!.Id);
            Assert.Equal(dto.Raca, resultado.Value.Raca);
            Assert.Equal(dto.IdEspecie, resultado.Value.IdEspecie);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task EditarAsync_QuandoEspecieInvalida_DeveRetornar404()
        {
            // Arrange
            int idRaca = 1;
            var dto = new RacaRequestDto { Raca = "Labrador Retriever", IdEspecie = 999 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((EspecieEntity?)null);

            // Act
            var resultado = await _racaUseCase.EditarAsync(idRaca, dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idRaca, resultado.Value!.Id);
            Assert.Equal(raca.Raca, resultado.Value.Raca);
            Assert.Equal("S", resultado.Value.StAtivo);
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idRaca, resultado.Value!.Id);
            Assert.Equal(raca.Raca, resultado.Value.Raca);
            Assert.Equal("N", resultado.Value.StAtivo);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _racaRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((RacaEntity?)null);

            // Act
            var resultado = await _racaUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterTodasAsync_QuandoListaVazia_DeveRetornar204()
        {
            // Arrange
            _racaRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PageResultModel<IEnumerable<RacaEntity>> { Data = new List<RacaEntity>() });

            // Act
            var resultado = await _racaUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Racas")]
        public async Task ObterTodasAsync_QuandoRepositorioFalha_DeveRetornar400SemVazarMensagem()
        {
            // Arrange
            _racaRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("ORA-12541: detalhe interno"));

            // Act
            var resultado = await _racaUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("ORA-12541", resultado.Error);
        }
    }
}
