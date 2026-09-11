using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class FeedbackNPSControllerTest
    {
        private readonly Mock<IFeedbackNPSUseCase> _useCase;
        private readonly HttpClient _client;

        public FeedbackNPSControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.FeedbackNPSUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task GetAll_ComListaDeFeedbacks_DeveRetornar200()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Id = 1, Nota = 10, IdResponsavel = 1 },
                new FeedbackNPSEntity { Id = 2, Nota = 7, IdAnimal = 5 }
            };
            _useCase.Setup(x => x.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = feedbacks, TotalRegistros = feedbacks.Count });

            // Act
            var response = await _client.GetAsync("/api/feedbacks-nps");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<FeedbackNPSEntity>>>();
            Assert.Equal(2, body!.Data.Count);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task GetAll_QuandoNaoHaFeedbacks_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<FeedbackNPSEntity>> { Data = new List<FeedbackNPSEntity>() });

            // Act
            var response = await _client.GetAsync("/api/feedbacks-nps");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task GetById_QuandoFeedbackExiste_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(8))
                .ReturnsAsync(new FeedbackNPSEntity { Id = 8, Nota = 9, IdResponsavel = 2 });

            // Act
            var response = await _client.GetAsync("/api/feedbacks-nps/8");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<FeedbackNPSEntity>();
            Assert.Equal(9, body!.Nota);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task GetById_QuandoFeedbackNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((FeedbackNPSEntity?)null);

            // Act
            var response = await _client.GetAsync("/api/feedbacks-nps/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task Create_QuandoContextoValido_DeveRetornar201()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<FeedbackNPSRequestDto>()))
                .ReturnsAsync(new FeedbackNPSEntity { Id = 40, Nota = 9, IdResponsavel = 1 });

            // Act
            var response = await _client.PostAsJsonAsync("/api/feedbacks-nps",
                new FeedbackNPSRequestDto { Nota = 9, IdResponsavel = 1 });

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task Create_QuandoContextoNaoEncontrado_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<FeedbackNPSRequestDto>()))
                .ThrowsAsync(new ContextoNaoEncontradoException("Responsável com ID 999 não encontrado."));

            // Act
            var response = await _client.PostAsJsonAsync("/api/feedbacks-nps",
                new FeedbackNPSRequestDto { Nota = 9, IdResponsavel = 999 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task Create_QuandoNenhumContextoInformado_DeveRetornar400()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<FeedbackNPSRequestDto>()))
                .ThrowsAsync(new ArgumentException("Informe ao menos um contexto."));

            // Act
            var response = await _client.PostAsJsonAsync("/api/feedbacks-nps",
                new FeedbackNPSRequestDto { Nota = 9 });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task Delete_QuandoRemovidoComSucesso_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.DeletarAsync(4))
                .ReturnsAsync(new FeedbackNPSEntity { Id = 4, Nota = 8 });

            // Act
            var response = await _client.DeleteAsync("/api/feedbacks-nps/4");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "FeedbackNPS")]
        public async Task Delete_QuandoFeedbackNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.DeletarAsync(It.IsAny<int>())).ReturnsAsync((FeedbackNPSEntity?)null);

            // Act
            var response = await _client.DeleteAsync("/api/feedbacks-nps/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
