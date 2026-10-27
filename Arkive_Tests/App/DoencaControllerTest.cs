using System.Net;
using System.Net.Http.Json;
using Arkive_Application.Dtos;
using Arkive_Application.Exceptions;
using Arkive_Application.Interfaces;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class DoencaControllerTest
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly Mock<IDoencaUseCase> _useCase;
        private readonly HttpClient _client;

        public DoencaControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _useCase = factory.DoencaUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateAuthenticatedClient();
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetAll_ComListaDeDoencas_DeveRetornar200()
        {
            // Arrange
            var doencas = new List<GetDoencaDto>
            {
                new GetDoencaDto { Id = 1, Nome = "Cinomose", StAtivo = "S" },
                new GetDoencaDto { Id = 2, Nome = "Raiva", StAtivo = "N" }
            };
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Success(
                    new PageResultModel<IEnumerable<GetDoencaDto>> { Data = doencas, TotalRegistros = doencas.Count }));

            // Act
            var response = await _client.GetAsync("/api/doencas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<GetDoencaDto>>>();
            Assert.Equal(2, body!.Data.Count);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetAll_QuandoNaoHaDoencas_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(OperationResult<PageResultModel<IEnumerable<GetDoencaDto>>>.Failure("Nenhuma doença encontrada", 204));

            // Act
            var response = await _client.GetAsync("/api/doencas");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetById_QuandoDoencaExiste_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(7))
                .ReturnsAsync(OperationResult<GetDoencaDto?>.Success(new GetDoencaDto { Id = 7, Nome = "Parvovirose", StAtivo = "S", Links = { new("/api/doencas/7", "self") } }));

            // Act
            var response = await _client.GetAsync("/api/doencas/7");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<GetDoencaDto>();
            Assert.Equal("Parvovirose", body!.Nome);
            Assert.Equal("self", body.Links.Single().Rel); // JSON plano com "links" no mesmo nível dos campos
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetById_QuandoDoencaNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<GetDoencaDto?>.Failure("Doença não encontrada", 404));

            // Act
            var response = await _client.GetAsync("/api/doencas/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task Create_QuandoDadosValidos_DeveRetornar201()
        {
            // Arrange
            var dto = new DoencaRequestDto { Nome = "Leptospirose" };
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<DoencaRequestDto>()))
                .ReturnsAsync(OperationResult<DoencaEntity?>.Success(new DoencaEntity { Id = 20, Nome = "Leptospirose", StAtivo = "S" }, 201));

            // Act
            var response = await _client.PostAsJsonAsync("/api/doencas", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task Create_QuandoCategoriaNaoEncontrada_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<DoencaRequestDto>()))
                .ReturnsAsync(OperationResult<DoencaEntity?>.Failure(new CategoriaNaoEncontradaException(999).Message, 404));

            // Act
            var response = await _client.PostAsJsonAsync("/api/doencas",
                new DoencaRequestDto { Nome = "X", IdCategoria = 999 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task Update_QuandoDoencaNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.EditarAsync(It.IsAny<int>(), It.IsAny<DoencaRequestDto>()))
                .ReturnsAsync(OperationResult<DoencaEntity?>.Failure("Doença não encontrada", 404));

            // Act
            var response = await _client.PutAsJsonAsync("/api/doencas/999", new DoencaRequestDto { Nome = "X" });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task Delete_QuandoInativadaComSucesso_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.InativarAsync(3))
                .ReturnsAsync(OperationResult<DoencaEntity?>.Success(new DoencaEntity { Id = 3, Nome = "Cinomose", StAtivo = "N" }));

            // Act
            var response = await _client.DeleteAsync("/api/doencas/3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetAll_SemToken_DeveRetornar401()
        {
            // Arrange
            using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // Act
            var response = await client.GetAsync("/api/doencas");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task Create_PerfilResponsavel_DeveRetornar403()
        {
            // Arrange
            using var client = _factory.CreateAuthenticatedClient("RESPONSAVEL");

            // Act
            var response = await client.PostAsJsonAsync("/api/doencas", new DoencaRequestDto { Nome = "Leptospirose" });

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task Create_PerfilVeterinario_DeveRetornar201()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<DoencaRequestDto>()))
                .ReturnsAsync(OperationResult<DoencaEntity?>.Success(new DoencaEntity { Id = 21, Nome = "Leptospirose", StAtivo = "S" }, 201));
            using var client = _factory.CreateAuthenticatedClient("VETERINARIO");

            // Act
            var response = await client.PostAsJsonAsync("/api/doencas", new DoencaRequestDto { Nome = "Leptospirose" });

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
