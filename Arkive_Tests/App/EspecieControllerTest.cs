using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class EspecieControllerTest
    {
        private readonly Mock<IEspecieUseCase> _useCase;
        private readonly HttpClient _client;

        public EspecieControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.EspecieUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task GetAll_ComListaDeEspecies_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(new List<EspecieEntity>
            {
                new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "S" }
            });

            // Act
            var response = await _client.GetAsync("/api/especies");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<List<EspecieEntity>>();
            Assert.Equal(2, body!.Count);
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task GetAll_QuandoNaoHaEspecies_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(new List<EspecieEntity>());

            // Act
            var response = await _client.GetAsync("/api/especies");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task GetById_QuandoEspecieExiste_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(4))
                .ReturnsAsync(new EspecieEntity { Id = 4, Especie = "Equina", StAtivo = "S" });

            // Act
            var response = await _client.GetAsync("/api/especies/4");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<EspecieEntity>();
            Assert.Equal("Equina", body!.Especie);
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task GetById_QuandoEspecieNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((EspecieEntity?)null);

            // Act
            var response = await _client.GetAsync("/api/especies/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task Create_QuandoDadosValidos_DeveRetornar201()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<EspecieRequestDto>()))
                .ReturnsAsync(new EspecieEntity { Id = 9, Especie = "Suína", StAtivo = "S" });

            // Act
            var response = await _client.PostAsJsonAsync("/api/especies", new EspecieRequestDto { Especie = "Suína" });

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task Update_QuandoEspecieNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.EditarAsync(It.IsAny<int>(), It.IsAny<EspecieRequestDto>()))
                .ReturnsAsync((EspecieEntity?)null);

            // Act
            var response = await _client.PutAsJsonAsync("/api/especies/999", new EspecieRequestDto { Especie = "X" });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Especies")]
        public async Task Delete_QuandoInativadaComSucesso_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.InativarAsync(2))
                .ReturnsAsync(new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "N" });

            // Act
            var response = await _client.DeleteAsync("/api/especies/2");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
