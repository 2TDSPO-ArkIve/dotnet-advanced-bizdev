using System.Net;
using System.Net.Http.Json;
using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class CategoriaDoencaControllerTest
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly Mock<Arkive_Application.Interfaces.ICategoriaDoencaUseCase> _useCase;
        private readonly HttpClient _client;

        public CategoriaDoencaControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _useCase = factory.CategoriaDoencaUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateAuthenticatedClient();
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetAll_ComListaDeCategorias_DeveRetornar200()
        {
            // Arrange
            var categorias = new List<GetCategoriaDoencaDto>
            {
                new GetCategoriaDoencaDto { Id = 1, Nome = "Viral", StAtivo = "S" },
                new GetCategoriaDoencaDto { Id = 2, Nome = "Bacteriana", StAtivo = "S" }
            };
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Success(new PageResultModel<IEnumerable<GetCategoriaDoencaDto>> { Data = categorias }));

            // Act
            var response = await _client.GetAsync("/api/categorias-doenca");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<GetCategoriaDoencaDto>>>();
            Assert.NotNull(body);
            Assert.Equal(2, body!.Data.Count);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetAll_QuandoNaoHaCategorias_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(OperationResult<PageResultModel<IEnumerable<GetCategoriaDoencaDto>>>.Failure("Nenhuma categoria de doença encontrada", 204));

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
            var categoria = new GetCategoriaDoencaDto { Id = 5, Nome = "Viral", StAtivo = "S" };
            _useCase.Setup(x => x.ObterPorIdAsync(5)).ReturnsAsync(OperationResult<GetCategoriaDoencaDto?>.Success(categoria));

            // Act
            var response = await _client.GetAsync("/api/categorias-doenca/5");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<GetCategoriaDoencaDto>();
            Assert.Equal(5, body!.Id);
            Assert.Equal("Viral", body.Nome);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task GetById_QuandoCategoriaNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(OperationResult<GetCategoriaDoencaDto?>.Failure("Categoria de doença não encontrada", 404));

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
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<CategoriaDoencaRequestDto>())).ReturnsAsync(OperationResult<CategoriaDoencaEntity?>.Success(criada, 201));

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
                .ReturnsAsync(OperationResult<CategoriaDoencaEntity?>.Failure("Categoria de doença não encontrada", 404));

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
            _useCase.Setup(x => x.InativarAsync(3)).ReturnsAsync(OperationResult<CategoriaDoencaEntity?>.Success(inativada));

            // Act
            var response = await _client.DeleteAsync("/api/categorias-doenca/3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "CategoriaDoenca")]
        public async Task Create_PerfilVeterinario_DeveRetornar403()
        {
            // Arrange
            using var client = _factory.CreateAuthenticatedClient("VETERINARIO");

            // Act
            var response = await client.PostAsJsonAsync("/api/categorias-doenca", new CategoriaDoencaRequestDto { Nome = "Parasitária" });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
