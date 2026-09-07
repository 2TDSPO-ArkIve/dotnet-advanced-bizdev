using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Exceptions;
using Arkive_API.Application.Interfaces;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Arkive_Tests.App
{
    public class PredisposicaoControllerTest : IClassFixture<CustomWebApplicationFactory>
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
        public async Task GetAll_DeveRetornar200_ComListaDePredisposicoes()
        {
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { Id = 1, IdEspecie = 1, IdDoenca = 1 },
                new PredisposicaoEntity { Id = 2, IdEspecie = 1, IdRaca = 3, IdDoenca = 2 }
            });

            var response = await _client.GetAsync("/api/predisposicoes");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<List<PredisposicaoEntity>>();
            Assert.Equal(2, body!.Count);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetAll_DeveRetornar204_QuandoNaoHaPredisposicoes()
        {
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<PredisposicaoEntity>());

            var response = await _client.GetAsync("/api/predisposicoes");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetById_DeveRetornar200_QuandoVinculoExiste()
        {
            _useCase.Setup(x => x.ObterPorIdAsync(5))
                .ReturnsAsync(new PredisposicaoEntity { Id = 5, IdEspecie = 1, IdDoenca = 2 });

            var response = await _client.GetAsync("/api/predisposicoes/5");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PredisposicaoEntity>();
            Assert.Equal(5, body!.Id);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task GetById_DeveRetornar404_QuandoVinculoNaoExiste()
        {
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            var response = await _client.GetAsync("/api/predisposicoes/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Create_DeveRetornar201_QuandoDadosValidos()
        {
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<PredisposicaoRequestDto>()))
                .ReturnsAsync(new PredisposicaoEntity { Id = 30, IdEspecie = 1, IdDoenca = 2 });

            var response = await _client.PostAsJsonAsync("/api/predisposicoes",
                new PredisposicaoRequestDto { IdEspecie = 1, IdDoenca = 2 });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Create_DeveRetornar404_QuandoDoencaNaoEncontrada()
        {
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<PredisposicaoRequestDto>()))
                .ThrowsAsync(new DoencaNaoEncontradaException(999));

            var response = await _client.PostAsJsonAsync("/api/predisposicoes",
                new PredisposicaoRequestDto { IdEspecie = 1, IdDoenca = 999 });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Delete_DeveRetornar200_QuandoRemovidaComSucesso()
        {
            _useCase.Setup(x => x.DeletarAsync(4))
                .ReturnsAsync(new PredisposicaoEntity { Id = 4, IdEspecie = 1, IdDoenca = 2 });

            var response = await _client.DeleteAsync("/api/predisposicoes/4");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Predisposicoes")]
        public async Task Delete_DeveRetornar404_QuandoVinculoNaoExiste()
        {
            _useCase.Setup(x => x.DeletarAsync(It.IsAny<int>())).ReturnsAsync((PredisposicaoEntity?)null);

            var response = await _client.DeleteAsync("/api/predisposicoes/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
