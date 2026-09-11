using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class CategoriaDoencaControllerTest
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly Mock<Arkive_API.Application.Interfaces.ICategoriaDoencaUseCase> _useCase;
        private readonly HttpClient _client;

        public CategoriaDoencaControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _useCase = factory.CategoriaDoencaUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetAll_ComListaDeCategorias_DeveRetornar200()
        {
            // Arrange
            var categorias = new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Id = 1, Nome = "Viral", StAtivo = "S" },
                new CategoriaDoencaEntity { Id = 2, Nome = "Bacteriana", StAtivo = "S" }
            };
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(categorias);

            // Act
            var response = await _client.GetAsync("/api/categorias-doenca");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<List<CategoriaDoencaEntity>>();
            Assert.NotNull(body);
            Assert.Equal(2, body!.Count);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetAll_QuandoNaoHaCategorias_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(new List<CategoriaDoencaEntity>());

            // Act
            var response = await _client.GetAsync("/api/categorias-doenca");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetById_QuandoCategoriaExiste_DeveRetornar200()
        {
            // Arrange
            var categoria = new CategoriaDoencaEntity { Id = 5, Nome = "Viral", StAtivo = "S" };
            _useCase.Setup(x => x.ObterPorIdAsync(5)).ReturnsAsync(categoria);

            // Act
            var response = await _client.GetAsync("/api/categorias-doenca/5");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<CategoriaDoencaEntity>();
            Assert.Equal(5, body!.Id);
            Assert.Equal("Viral", body.Nome);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetById_QuandoCategoriaNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var response = await _client.GetAsync("/api/categorias-doenca/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Create_QuandoDadosValidos_DeveRetornar201()
        {
            // Arrange
            var dto = new CategoriaDoencaRequestDto { Nome = "Parasitária" };
            var criada = new CategoriaDoencaEntity { Id = 10, Nome = "Parasitária", StAtivo = "S" };
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<CategoriaDoencaRequestDto>())).ReturnsAsync(criada);

            // Act
            var response = await _client.PostAsJsonAsync("/api/categorias-doenca", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<CategoriaDoencaEntity>();
            Assert.Equal(10, body!.Id);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Update_QuandoCategoriaNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.EditarAsync(It.IsAny<int>(), It.IsAny<CategoriaDoencaRequestDto>()))
                .ReturnsAsync((CategoriaDoencaEntity?)null);

            // Act
            var response = await _client.PutAsJsonAsync("/api/categorias-doenca/999",
                new CategoriaDoencaRequestDto { Nome = "Nova" });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Delete_QuandoInativadaComSucesso_DeveRetornar200()
        {
            // Arrange
            var inativada = new CategoriaDoencaEntity { Id = 3, Nome = "Viral", StAtivo = "N" };
            _useCase.Setup(x => x.InativarAsync(3)).ReturnsAsync(inativada);

            // Act
            var response = await _client.DeleteAsync("/api/categorias-doenca/3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
