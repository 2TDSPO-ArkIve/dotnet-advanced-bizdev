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
    public class PredisposicaoUseCaseTest
    {
        private readonly Mock<IPredisposicaoRepository> _predisposicaoRepository;
        private readonly Mock<IEspecieRepository> _especieRepository;
        private readonly Mock<IRacaRepository> _racaRepository;
        private readonly Mock<IDoencaRepository> _doencaRepository;
        private readonly PredisposicaoUseCase _predisposicaoUseCase;

        public PredisposicaoUseCaseTest()
        {
            _predisposicaoRepository = new Mock<IPredisposicaoRepository>();
            _especieRepository = new Mock<IEspecieRepository>();
            _racaRepository = new Mock<IRacaRepository>();
            _doencaRepository = new Mock<IDoencaRepository>();

            // Isso é o que vamos testar
            _predisposicaoUseCase = new PredisposicaoUseCase(
                _predisposicaoRepository.Object,
                _especieRepository.Object,
                _racaRepository.Object,
                _doencaRepository.Object,
                NullLogger<PredisposicaoUseCase>.Instance);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterTodasAsync_DeveRetornarTodasAsPredisposicoes()
        {
            // Arrange
            var predisposicoes = new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { Id = 1, IdEspecie = 1, IdDoenca = 1 },
                new PredisposicaoEntity { Id = 2, IdEspecie = 2, IdDoenca = 2 }
            };

            _predisposicaoRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item =>
                {
                    Assert.Equal(1, item.Id);
                    Assert.Equal(1, item.IdEspecie);
                    Assert.Null(item.IdRaca);
                    Assert.Equal(1, item.IdDoenca);
                },
                item =>
                {
                    Assert.Equal(2, item.Id);
                    Assert.Equal(2, item.IdEspecie);
                    Assert.Null(item.IdRaca);
                    Assert.Equal(2, item.IdDoenca);
                });
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterPorIdAsync_DeveRetornarUmaPredisposicao()
        {
            // Arrange
            int idPredisposicao = 1;
            var predisposicao = new PredisposicaoEntity { Id = idPredisposicao, IdEspecie = 1, IdDoenca = 1 };

            _predisposicaoRepository.Setup(obj => obj.ObterPorIdAsync(idPredisposicao)).ReturnsAsync(predisposicao);

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorIdAsync(idPredisposicao);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idPredisposicao, resultado!.Id);
            Assert.Equal(1, resultado.IdEspecie);
            Assert.Equal(1, resultado.IdDoenca);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterPorEspecieAsync_DeveRetornarPredisposicoesDaEspecie()
        {
            // Arrange
            int idEspecie = 1;
            var predisposicoes = new List<PredisposicaoEntity> { new PredisposicaoEntity { Id = 1, IdEspecie = idEspecie, IdDoenca = 1 } };

            _predisposicaoRepository.Setup(obj => obj.ObterPorEspecieAsync(idEspecie, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorEspecieAsync(idEspecie);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data, item =>
            {
                Assert.Equal(1, item.Id);
                Assert.Equal(idEspecie, item.IdEspecie);
                Assert.Equal(1, item.IdDoenca);
            });
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterPorRacaAsync_DeveRetornarPredisposicoesDaRaca()
        {
            // Arrange
            int idRaca = 1;
            var predisposicoes = new List<PredisposicaoEntity> { new PredisposicaoEntity { Id = 1, IdEspecie = 1, IdRaca = idRaca, IdDoenca = 1 } };

            _predisposicaoRepository.Setup(obj => obj.ObterPorRacaAsync(idRaca, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorRacaAsync(idRaca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data, item =>
            {
                Assert.Equal(1, item.Id);
                Assert.Equal(1, item.IdEspecie);
                Assert.Equal(idRaca, item.IdRaca);
                Assert.Equal(1, item.IdDoenca);
            });
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterPorDoencaAsync_DeveRetornarPredisposicoesDaDoenca()
        {
            // Arrange
            int idDoenca = 1;
            var predisposicoes = new List<PredisposicaoEntity> { new PredisposicaoEntity { Id = 1, IdEspecie = 1, IdDoenca = idDoenca } };

            _predisposicaoRepository.Setup(obj => obj.ObterPorDoencaAsync(idDoenca, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorDoencaAsync(idDoenca);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data, item =>
            {
                Assert.Equal(1, item.Id);
                Assert.Equal(1, item.IdEspecie);
                Assert.Equal(idDoenca, item.IdDoenca);
            });
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoTudoValidoSemRaca_DeveAdicionarPredisposicao()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 1, IdRaca = null, IdDoenca = 1 };
            var entity = dto.ToPredisposicaoEntity();

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" });
            _doencaRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new DoencaEntity { Id = 1, Nome = "Cinomose", StAtivo = "S" });
            _predisposicaoRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _predisposicaoUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.IdEspecie, resultado!.IdEspecie);
            Assert.Equal(dto.IdDoenca, resultado.IdDoenca);
            Assert.Null(resultado.IdRaca);

            _racaRepository.Verify(obj => obj.ObterPorIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoTudoValidoComRaca_DeveAdicionarPredisposicao()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 1, IdRaca = 1, IdDoenca = 1 };
            var entity = dto.ToPredisposicaoEntity();

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" });
            _racaRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new RacaEntity { Id = 1, Raca = "Labrador", StAtivo = "S" });
            _doencaRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new DoencaEntity { Id = 1, Nome = "Cinomose", StAtivo = "S" });
            _predisposicaoRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _predisposicaoUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.IdRaca, resultado!.IdRaca);
            Assert.Equal(dto.IdEspecie, resultado.IdEspecie);
            Assert.Equal(dto.IdDoenca, resultado.IdDoenca);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoEspecieNaoExisteOuInativa_DeveLancarExcecao()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 999, IdDoenca = 1 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((EspecieEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<EspecieNaoEncontradaException>(() => _predisposicaoUseCase.AdicionarAsync(dto));

            _predisposicaoRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoRacaNaoExisteOuInativa_DeveLancarExcecao()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 1, IdRaca = 999, IdDoenca = 1 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" });
            _racaRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((RacaEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<RacaNaoEncontradaException>(() => _predisposicaoUseCase.AdicionarAsync(dto));

            _predisposicaoRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoDoencaNaoExisteOuInativa_DeveLancarExcecao()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 1, IdDoenca = 999 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" });
            _doencaRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((DoencaEntity?)null);

            // Act & Assert
            await Assert.ThrowsAsync<DoencaNaoEncontradaException>(() => _predisposicaoUseCase.AdicionarAsync(dto));

            _predisposicaoRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task DeletarAsync_DeveDeletarPredisposicao()
        {
            // Arrange
            int idPredisposicao = 1;
            var predisposicao = new PredisposicaoEntity { Id = idPredisposicao, IdEspecie = 1, IdDoenca = 1 };

            _predisposicaoRepository.Setup(obj => obj.DeletarAsync(idPredisposicao)).ReturnsAsync(predisposicao);

            // Act
            var resultado = await _predisposicaoUseCase.DeletarAsync(idPredisposicao);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idPredisposicao, resultado!.Id);
            Assert.Equal(1, resultado.IdEspecie);
            Assert.Equal(1, resultado.IdDoenca);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task DeletarAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            // Arrange
            _predisposicaoRepository.Setup(obj => obj.DeletarAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            // Act
            var resultado = await _predisposicaoUseCase.DeletarAsync(999);

            // Assert
            Assert.Null(resultado);
        }
    }
}
