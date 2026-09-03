using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Mappers;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Interfaces;
using Moq;

namespace Arkive_Tests.App
{
    public class FeedbackNPSUseCaseTest
    {
        private readonly Mock<IFeedbackNPSRepository> _feedbackNPSRepository;
        private readonly FeedbackNPSUseCase _feedbackNPSUseCase;

        public FeedbackNPSUseCaseTest()
        {
            _feedbackNPSRepository = new Mock<IFeedbackNPSRepository>();

            // Isso é o que vamos testar
            _feedbackNPSUseCase = new FeedbackNPSUseCase(_feedbackNPSRepository.Object);

            // Por padrão, todos os contextos existem — os testes de exceção sobrescrevem isso
            _feedbackNPSRepository.Setup(obj => obj.ResponsavelExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
            _feedbackNPSRepository.Setup(obj => obj.AnimalExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
            _feedbackNPSRepository.Setup(obj => obj.ClinicaExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
            _feedbackNPSRepository.Setup(obj => obj.ConsultaExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
            _feedbackNPSRepository.Setup(obj => obj.VeterinarioExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterTodosAsync_DeveRetornarTodosOsFeedbacks()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Id = 1, Nota = 10 },
                new FeedbackNPSEntity { Id = 2, Nota = 5 }
            };

            _feedbackNPSRepository.Setup(obj => obj.ObterTodosAsync()).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorIdAsync_DeveRetornarUmFeedback()
        {
            // Arrange
            int idFeedback = 1;
            var feedback = new FeedbackNPSEntity { Id = idFeedback, Nota = 10 };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorIdAsync(idFeedback)).ReturnsAsync(feedback);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorIdAsync(idFeedback);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idFeedback, resultado!.Id);
        }

        [Theory]
        [Trait("UseCase", "FeedbackNPS")]
        [InlineData(0)]
        [InlineData(10)]
        public async Task ObterPorNotaAsync_DeveRetornarFeedbacks_QuandoNotaValida(int nota)
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, Nota = nota } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorNotaAsync(nota)).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorNotaAsync(nota);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Theory]
        [Trait("UseCase", "FeedbackNPS")]
        [InlineData(-1)]
        [InlineData(11)]
        public async Task ObterPorNotaAsync_DeveLancarExcecao_QuandoNotaForaDoIntervalo(int nota)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _feedbackNPSUseCase.ObterPorNotaAsync(nota));

            _feedbackNPSRepository.Verify(obj => obj.ObterPorNotaAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorResponsavelAsync_DeveRetornarFeedbacksDoResponsavel()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdResponsavel = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorResponsavelAsync(1)).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorResponsavelAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorAnimalAsync_DeveRetornarFeedbacksDoAnimal()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdAnimal = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorAnimalAsync(1)).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorAnimalAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorClinicaAsync_DeveRetornarFeedbacksDaClinica()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdClinica = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorClinicaAsync(1)).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorClinicaAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorVeterinarioAsync_DeveRetornarFeedbacksDoVeterinario()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdVeterinario = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorVeterinarioAsync(1)).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorVeterinarioAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorDataAsync_DeveRetornarFeedbacksDaData()
        {
            // Arrange
            var data = DateTime.Today;
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, Nota = 10, DataFeedback = data } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorDataAsync(data)).ReturnsAsync(feedbacks);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorDataAsync(data);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoNenhumContextoInformado()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8 };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _feedbackNPSUseCase.AdicionarAsync(dto));

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoResponsavelNaoExiste()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdResponsavel = 1 };

            _feedbackNPSRepository.Setup(obj => obj.ResponsavelExisteAsync(1)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ContextoNaoEncontradoException>(() => _feedbackNPSUseCase.AdicionarAsync(dto));

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoAnimalNaoExiste()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdAnimal = 1 };

            _feedbackNPSRepository.Setup(obj => obj.AnimalExisteAsync(1)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ContextoNaoEncontradoException>(() => _feedbackNPSUseCase.AdicionarAsync(dto));

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoClinicaNaoExiste()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdClinica = 1 };

            _feedbackNPSRepository.Setup(obj => obj.ClinicaExisteAsync(1)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ContextoNaoEncontradoException>(() => _feedbackNPSUseCase.AdicionarAsync(dto));

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoConsultaNaoExiste()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdConsulta = 1 };

            _feedbackNPSRepository.Setup(obj => obj.ConsultaExisteAsync(1)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ContextoNaoEncontradoException>(() => _feedbackNPSUseCase.AdicionarAsync(dto));

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveLancarExcecao_QuandoVeterinarioNaoExiste()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdVeterinario = 1 };

            _feedbackNPSRepository.Setup(obj => obj.VeterinarioExisteAsync(1)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ContextoNaoEncontradoException>(() => _feedbackNPSUseCase.AdicionarAsync(dto));

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveAdicionarFeedback_QuandoContextoValido()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 9, IdResponsavel = 1, Comentario = "Ótimo atendimento" };
            var entity = dto.ToFeedbackNPSEntity();

            _feedbackNPSRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Nota, resultado!.Nota);
            Assert.Equal(dto.Comentario, resultado.Comentario);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task DeletarAsync_DeveDeletarFeedback()
        {
            // Arrange
            int idFeedback = 1;
            var feedback = new FeedbackNPSEntity { Id = idFeedback, Nota = 9 };

            _feedbackNPSRepository.Setup(obj => obj.DeletarAsync(idFeedback)).ReturnsAsync(feedback);

            // Act
            var resultado = await _feedbackNPSUseCase.DeletarAsync(idFeedback);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(idFeedback, resultado!.Id);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task DeletarAsync_DeveRetornarNull_QuandoFeedbackNaoExiste()
        {
            // Arrange
            _feedbackNPSRepository.Setup(obj => obj.DeletarAsync(It.IsAny<int>())).ReturnsAsync((FeedbackNPSEntity?)null);

            // Act
            var resultado = await _feedbackNPSUseCase.DeletarAsync(999);

            // Assert
            Assert.Null(resultado);
        }
    }
}
