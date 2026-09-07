using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Moq;

namespace Arkive_Tests.App
{
    public class CategoriaDoencaControllerTest : IClassFixture<CustomWebApplicationFactory>
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
        public async Task GetAll_DeveRetornar200_ComListaDeCategorias()
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
        public async Task GetAll_DeveRetornar204_QuandoNaoHaCategorias()
        {
            _useCase.Setup(x => x.ObterTodasAsync()).ReturnsAsync(new List<CategoriaDoencaEntity>());

            var response = await _client.GetAsync("/api/categorias-doenca");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetById_DeveRetornar200_QuandoCategoriaExiste()
        {
            var categoria = new CategoriaDoencaEntity { Id = 5, Nome = "Viral", StAtivo = "S" };
            _useCase.Setup(x => x.ObterPorIdAsync(5)).ReturnsAsync(categoria);

            var response = await _client.GetAsync("/api/categorias-doenca/5");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<CategoriaDoencaEntity>();
            Assert.Equal(5, body!.Id);
            Assert.Equal("Viral", body.Nome);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetById_DeveRetornar404_QuandoCategoriaNaoExiste()
        {
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((CategoriaDoencaEntity?)null);

            var response = await _client.GetAsync("/api/categorias-doenca/999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Create_DeveRetornar201_QuandoDadosValidos()
        {
            var dto = new CategoriaDoencaRequestDto { Nome = "Parasitária" };
            var criada = new CategoriaDoencaEntity { Id = 10, Nome = "Parasitária", StAtivo = "S" };
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<CategoriaDoencaRequestDto>())).ReturnsAsync(criada);

            var response = await _client.PostAsJsonAsync("/api/categorias-doenca", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<CategoriaDoencaEntity>();
            Assert.Equal(10, body!.Id);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Update_DeveRetornar404_QuandoCategoriaNaoExisteOuInativa()
        {
            _useCase.Setup(x => x.EditarAsync(It.IsAny<int>(), It.IsAny<CategoriaDoencaRequestDto>()))
                .ReturnsAsync((CategoriaDoencaEntity?)null);

            var response = await _client.PutAsJsonAsync("/api/categorias-doenca/999",
                new CategoriaDoencaRequestDto { Nome = "Nova" });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Delete_DeveRetornar200_QuandoInativadaComSucesso()
        {
            var inativada = new CategoriaDoencaEntity { Id = 3, Nome = "Viral", StAtivo = "N" };
            _useCase.Setup(x => x.InativarAsync(3)).ReturnsAsync(inativada);

            var response = await _client.DeleteAsync("/api/categorias-doenca/3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
