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
    public class FeedbackNPSUseCaseTest
    {
        private readonly Mock<IFeedbackNPSRepository> _feedbackNPSRepository;
        private readonly FeedbackNPSUseCase _feedbackNPSUseCase;

        public FeedbackNPSUseCaseTest()
        {
            _feedbackNPSRepository = new Mock<IFeedbackNPSRepository>();

            // Isso é o que vamos testar
            _feedbackNPSUseCase = new FeedbackNPSUseCase(_feedbackNPSRepository.Object, NullLogger<FeedbackNPSUseCase>.Instance);

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

            _feedbackNPSRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterTodosAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(10, item.Nota); },
                item => { Assert.Equal(2, item.Id); Assert.Equal(5, item.Nota); });
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idFeedback, resultado.Value!.Id);
            Assert.Equal(feedback.Nota, resultado.Value.Nota);
            Assert.Equal(new[] { "self", "delete_feedback_nps" }, resultado.Value.Links.Select(l => l.Rel));
        }

        [Theory]
        [Trait("UseCase", "FeedbackNPS")]
        [InlineData(0)]
        [InlineData(5)]
        public async Task ObterPorNotaAsync_QuandoNotaValida_DeveRetornarFeedbacks(int nota)
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, Nota = nota } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorNotaAsync(nota, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorNotaAsync(nota);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(nota, item.Nota); });
        }

        [Theory]
        [Trait("UseCase", "FeedbackNPS")]
        [InlineData(-1)]
        [InlineData(6)]
        public async Task ObterPorNotaAsync_QuandoNotaForaDoIntervalo_DeveRetornar400(int nota)
        {
            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorNotaAsync(nota);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.ObterPorNotaAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorResponsavelAsync_DeveRetornarFeedbacksDoResponsavel()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdResponsavel = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorResponsavelAsync(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorResponsavelAsync(1);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(1, item.IdResponsavel); Assert.Equal(10, item.Nota); });
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorAnimalAsync_DeveRetornarFeedbacksDoAnimal()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdAnimal = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorAnimalAsync(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorAnimalAsync(1);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(1, item.IdAnimal); Assert.Equal(10, item.Nota); });
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorClinicaAsync_DeveRetornarFeedbacksDaClinica()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdClinica = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorClinicaAsync(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorClinicaAsync(1);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(1, item.IdClinica); Assert.Equal(10, item.Nota); });
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorVeterinarioAsync_DeveRetornarFeedbacksDoVeterinario()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, IdVeterinario = 1, Nota = 10 } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorVeterinarioAsync(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorVeterinarioAsync(1);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(1, item.IdVeterinario); Assert.Equal(10, item.Nota); });
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorDataAsync_DeveRetornarFeedbacksDaData()
        {
            // Arrange
            var data = DateTime.Today;
            var feedbacks = new List<FeedbackNPSEntity> { new FeedbackNPSEntity { Id = 1, Nota = 10, DataFeedback = data } };

            _feedbackNPSRepository.Setup(obj => obj.ObterPorDataAsync(data, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorDataAsync(data);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Collection(resultado.Value!.Data,
                item => { Assert.Equal(1, item.Id); Assert.Equal(10, item.Nota); Assert.Equal(data, item.DataFeedback); });
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoNenhumContextoInformado_DeveRetornar400()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8 };

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoResponsavelNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdResponsavel = 1 };

            _feedbackNPSRepository.Setup(obj => obj.ResponsavelExisteAsync(1)).ReturnsAsync(false);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoAnimalNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdAnimal = 1 };

            _feedbackNPSRepository.Setup(obj => obj.AnimalExisteAsync(1)).ReturnsAsync(false);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoClinicaNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdClinica = 1 };

            _feedbackNPSRepository.Setup(obj => obj.ClinicaExisteAsync(1)).ReturnsAsync(false);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoConsultaNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdConsulta = 1 };

            _feedbackNPSRepository.Setup(obj => obj.ConsultaExisteAsync(1)).ReturnsAsync(false);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoVeterinarioNaoExiste_DeveRetornar404()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdVeterinario = 1 };

            _feedbackNPSRepository.Setup(obj => obj.VeterinarioExisteAsync(1)).ReturnsAsync(false);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);

            _feedbackNPSRepository.Verify(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>()), Times.Never);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task AdicionarAsync_QuandoContextoValido_DeveAdicionarFeedback()
        {
            // Arrange
            var dto = new FeedbackNPSRequestDto { Nota = 9, IdResponsavel = 1, Comentario = "Ótimo atendimento" };
            var entity = dto.ToFeedbackNPSEntity();

            _feedbackNPSRepository.Setup(obj => obj.AdicionarAsync(It.IsAny<FeedbackNPSEntity>())).ReturnsAsync(entity);

            // Act
            var resultado = await _feedbackNPSUseCase.AdicionarAsync(dto);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(201, resultado.StatusCode);
            Assert.Equal(dto.Nota, resultado.Value!.Nota);
            Assert.Equal(dto.Comentario, resultado.Value.Comentario);
            Assert.Equal(dto.IdResponsavel, resultado.Value.IdResponsavel);
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
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idFeedback, resultado.Value!.Id);
            Assert.Equal(feedback.Nota, resultado.Value.Nota);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task DeletarAsync_QuandoFeedbackNaoExiste_DeveRetornar404()
        {
            // Arrange
            _feedbackNPSRepository.Setup(obj => obj.DeletarAsync(It.IsAny<int>())).ReturnsAsync((FeedbackNPSEntity?)null);

            // Act
            var resultado = await _feedbackNPSUseCase.DeletarAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _feedbackNPSRepository.Setup(obj => obj.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((FeedbackNPSEntity?)null);

            // Act
            var resultado = await _feedbackNPSUseCase.ObterPorIdAsync(999);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterTodosAsync_QuandoListaVazia_DeveRetornar204()
        {
            // Arrange
            _feedbackNPSRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = new List<FeedbackNPSEntity>() });

            // Act
            var resultado = await _feedbackNPSUseCase.ObterTodosAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "FeedbackNPS")]
        public async Task ObterTodosAsync_QuandoRepositorioFalha_DeveRetornar400SemVazarMensagem()
        {
            // Arrange
            _feedbackNPSRepository.Setup(obj => obj.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("ORA-12541: detalhe interno"));

            // Act
            var resultado = await _feedbackNPSUseCase.ObterTodosAsync();

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("ORA-12541", resultado.Error);
        }
    }
}
