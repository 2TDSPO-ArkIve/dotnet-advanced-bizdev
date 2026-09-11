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
    public class PredisposicaoControllerTest
    {
        private readonly Mock<IPredisposicaoUseCase> _useCase;
        private readonly HttpClient _client;

        public PredisposicaoControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.PredisposicaoUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetAll_ComListaDePredisposicoes_DeveRetornar200()
        {
            // Arrange
            var predisposicoes = new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { Id = 1, IdEspecie = 1, IdDoenca = 1 },
                new PredisposicaoEntity { Id = 2, IdEspecie = 1, IdRaca = 3, IdDoenca = 2 }
            };
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = predisposicoes, TotalRegistros = predisposicoes.Count });

            // Act
            var response = await _client.GetAsync("/api/predisposicoes");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<PredisposicaoEntity>>>();
            Assert.Equal(2, body!.Data.Count);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetAll_QuandoNaoHaPredisposicoes_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<PredisposicaoEntity>> { Data = new List<PredisposicaoEntity>() });

            // Act
            var response = await _client.GetAsync("/api/predisposicoes");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetById_QuandoVinculoExiste_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(5))
                .ReturnsAsync(new PredisposicaoEntity { Id = 5, IdEspecie = 1, IdDoenca = 2 });

            // Act
            var response = await _client.GetAsync("/api/predisposicoes/5");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PredisposicaoEntity>();
            Assert.Equal(5, body!.Id);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetById_QuandoVinculoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            // Act
            var response = await _client.GetAsync("/api/predisposicoes/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Create_QuandoDadosValidos_DeveRetornar201()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<PredisposicaoRequestDto>()))
                .ReturnsAsync(new PredisposicaoEntity { Id = 30, IdEspecie = 1, IdDoenca = 2 });

            // Act
            var response = await _client.PostAsJsonAsync("/api/predisposicoes",
                new PredisposicaoRequestDto { IdEspecie = 1, IdDoenca = 2 });

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Create_QuandoDoencaNaoEncontrada_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<PredisposicaoRequestDto>()))
                .ThrowsAsync(new DoencaNaoEncontradaException(999));

            // Act
            var response = await _client.PostAsJsonAsync("/api/predisposicoes",
                new PredisposicaoRequestDto { IdEspecie = 1, IdDoenca = 999 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Delete_QuandoRemovidaComSucesso_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.DeletarAsync(4))
                .ReturnsAsync(new PredisposicaoEntity { Id = 4, IdEspecie = 1, IdDoenca = 2 });

            // Act
            var response = await _client.DeleteAsync("/api/predisposicoes/4");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Delete_QuandoVinculoNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.DeletarAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            // Act
            var response = await _client.DeleteAsync("/api/predisposicoes/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
