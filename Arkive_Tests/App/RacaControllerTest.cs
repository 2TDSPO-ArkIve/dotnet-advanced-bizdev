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
    public class RacaControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly Mock<IRacaUseCase> _useCase;
        private readonly HttpClient _client;

        public RacaControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.RacaUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetAll_DeveRetornar200_ComListaDeRacas()
        {
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(new List<RacaEntity>
            {
                new RacaEntity { Id = 1, Raca = "Poodle", IdEspecie = 1, StAtivo = "S" },
                new RacaEntity { Id = 2, Raca = "Siamês", IdEspecie = 2, StAtivo = "S" }
            });

            var response = await _client.GetAsync("/api/racas");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<List<RacaEntity>>();
            Assert.Equal(2, body!.Count);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetAll_DeveRetornar204_QuandoNaoHaRacas()
        {
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(new List<RacaEntity>());

            var response = await _client.GetAsync("/api/racas");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetById_DeveRetornar200_QuandoRacaExiste()
        {
            _useCase.Setup(x => x.ObterPorIdAsync(3))
                .ReturnsAsync(new RacaEntity { Id = 3, Raca = "Bulldog", IdEspecie = 1, StAtivo = "S" });

            var response = await _client.GetAsync("/api/racas/3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<RacaEntity>();
            Assert.Equal("Bulldog", body!.Raca);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetById_DeveRetornar404_QuandoRacaNaoExiste()
        {
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((RacaEntity?)null);

            var response = await _client.GetAsync("/api/racas/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Create_DeveRetornar201_QuandoDadosValidos()
        {
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<RacaRequestDto>()))
                .ReturnsAsync(new RacaEntity { Id = 15, Raca = "Beagle", IdEspecie = 1, StAtivo = "S" });

            var response = await _client.PostAsJsonAsync("/api/racas",
                new RacaRequestDto { Raca = "Beagle", IdEspecie = 1 });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Create_DeveRetornar404_QuandoEspecieNaoEncontrada()
        {
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<RacaRequestDto>()))
                .ThrowsAsync(new EspecieNaoEncontradaException(999));

            var response = await _client.PostAsJsonAsync("/api/racas",
                new RacaRequestDto { Raca = "X", IdEspecie = 999 });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Update_DeveRetornar404_QuandoRacaNaoExisteOuInativa()
        {
            _useCase.Setup(x => x.EditarAsync(It.IsAny<int>(), It.IsAny<RacaRequestDto>()))
                .ReturnsAsync((RacaEntity?)null);

            var response = await _client.PutAsJsonAsync("/api/racas/999",
                new RacaRequestDto { Raca = "X", IdEspecie = 1 });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Delete_DeveRetornar200_QuandoInativadaComSucesso()
        {
            _useCase.Setup(x => x.InativarAsync(3))
                .ReturnsAsync(new RacaEntity { Id = 3, Raca = "Bulldog", IdEspecie = 1, StAtivo = "N" });

            var response = await _client.DeleteAsync("/api/racas/3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
