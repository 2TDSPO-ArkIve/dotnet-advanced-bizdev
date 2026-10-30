using Arkive_Application.Dtos;
using Arkive_Application.Mappers;
using Arkive_Application.UseCases;
using Arkive_Domain.Entities;
using Arkive_Domain.Interfaces;
using Arkive_Domain.Models;
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

            _predisposicaoRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterTodasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.All(resultado.Value!.Data, item => Assert.Equal(new[] { "get_predisposicao", "delete_predisposicao" }, item.Links.Select(l => l.Rel)));
            Assert.Collection(resultado.Value!.Data,
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idPredisposicao, resultado.Value!.Id);
            Assert.Equal(1, resultado.Value.IdEspecie);
            Assert.Equal(1, resultado.Value.IdDoenca);
            Assert.Equal(new[] { "self", "delete_predisposicao" }, resultado.Value.Links.Select(l => l.Rel));
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterPorEspecieAsync_DeveRetornarPredisposicoesDaEspecie()
        {
            // Arrange
            int idEspecie = 1;
            var predisposicoes = new List<PredisposicaoEntity> { new PredisposicaoEntity { Id = 1, IdEspecie = idEspecie, IdDoenca = 1 } };

            _predisposicaoRepository.Setup(obj => obj.ObterPorEspecieAsync(idEspecie, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorEspecieAsync(idEspecie);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data, item =>
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

            _predisposicaoRepository.Setup(obj => obj.ObterPorRacaAsync(idRaca, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorRacaAsync(idRaca);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data, item =>
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

            _predisposicaoRepository.Setup(obj => obj.ObterPorDoencaAsync(idDoenca, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorDoencaAsync(idDoenca);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data, item =>
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.IdEspecie, resultado.Value!.IdEspecie);
            Assert.Equal(dto.IdDoenca, resultado.Value.IdDoenca);
            Assert.Null(resultado.Value.IdRaca);

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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(dto.IdRaca, resultado.Value!.IdRaca);
            Assert.Equal(dto.IdEspecie, resultado.Value.IdEspecie);
            Assert.Equal(dto.IdDoenca, resultado.Value.IdDoenca);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoEspecieNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 999, IdDoenca = 1 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((EspecieEntity?)null);

            // Act
            var resultado = await _predisposicaoUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _predisposicaoRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoRacaNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 1, IdRaca = 999, IdDoenca = 1 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" });
            _racaRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((RacaEntity?)null);

            // Act
            var resultado = await _predisposicaoUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _predisposicaoRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<PredisposicaoEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task AdicionarAsync_QuandoDoencaNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            var dto = new PredisposicaoRequestDto { IdEspecie = 1, IdDoenca = 999 };

            _especieRepository.Setup(obj => obj.ObterPorIdAsync(1)).ReturnsAsync(new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" });
            _doencaRepository.Setup(obj => obj.ObterPorIdAsync(999)).ReturnsAsync((DoencaEntity?)null);

            // Act
            var resultado = await _predisposicaoUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idPredisposicao, resultado.Value!.Id);
            Assert.Equal(1, resultado.Value.IdEspecie);
            Assert.Equal(1, resultado.Value.IdDoenca);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task DeletarAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _predisposicaoRepository.Setup(obj => obj.DeletarAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            // Act
            var resultado = await _predisposicaoUseCase.DeletarAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _predisposicaoRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            // Act
            var resultado = await _predisposicaoUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterTodasAsync_QuandoListaVazia_DeveRetornar204()
        {
            // Arrange
            _predisposicaoRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = new List<PredisposicaoEntity>() });

            // Act
            var resultado = await _predisposicaoUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Predisposicoes")]
        public async Task ObterTodasAsync_QuandoRepositorioFalha_DeveRetornar400SemVazarMensagem()
        {
            // Arrange
            _predisposicaoRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("ORA-12541: detalhe interno"));

            // Act
            var resultado = await _predisposicaoUseCase.ObterTodasAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("ORA-12541", resultado.Error);
        }
    }
}
