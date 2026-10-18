using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class RacaControllerTest
    {
        private readonly Mock<IRacaUseCase> _useCase;
        private readonly HttpClient _client;

        public RacaControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.RacaUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateAuthenticatedClient();
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetAll_ComListaDeRacas_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(OperationResult<PageResultModel<IEnumerable<RacaEntity>>>.Success(new PageResultModel<IEnumerable<RacaEntity>> { Data = new List<RacaEntity>
            {
                new RacaEntity { Id = 1, Raca = "Poodle", IdEspecie = 1, StAtivo = "S" },
                new RacaEntity { Id = 2, Raca = "Siamês", IdEspecie = 2, StAtivo = "S" }
            } }));

            // Act
            var response = await _client.GetAsync("/api/racas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<RacaEntity>>>();
            Assert.Equal(2, body!.Data.Count);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetAll_QuandoNaoHaRacas_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(OperationResult<PageResultModel<IEnumerable<RacaEntity>>>.Failure("Nenhuma raça encontrada", 204));

            // Act
            var response = await _client.GetAsync("/api/racas");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetById_QuandoRacaExiste_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(3))
                .ReturnsAsync(OperationResult<GetRacaDto?>.Success(new GetRacaDto { Id = 3, Raca = "Bulldog", IdEspecie = 1, StAtivo = "S" }));

            // Act
            var response = await _client.GetAsync("/api/racas/3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<GetRacaDto>();
            Assert.Equal("Bulldog", body!.Raca);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task GetById_QuandoRacaNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync(OperationResult<GetRacaDto?>.Failure("Raça não encontrada", 404));

            // Act
            var response = await _client.GetAsync("/api/racas/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Create_QuandoDadosValidos_DeveRetornar201()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<RacaRequestDto>()))
                .ReturnsAsync(OperationResult<RacaEntity?>.Success(new RacaEntity { Id = 15, Raca = "Beagle", IdEspecie = 1, StAtivo = "S" }, 201));

            // Act
            var response = await _client.PostAsJsonAsync("/api/racas",
                new RacaRequestDto { Raca = "Beagle", IdEspecie = 1 });

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Create_QuandoEspecieNaoEncontrada_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.AdicionarAsync(It.IsAny<RacaRequestDto>()))
                .ReturnsAsync(OperationResult<RacaEntity?>.Failure(new Arkive_API.Application.Exceptions.EspecieNaoEncontradaException(999).Message, 404));

            // Act
            var response = await _client.PostAsJsonAsync("/api/racas",
                new RacaRequestDto { Raca = "X", IdEspecie = 999 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Update_QuandoRacaNaoExisteOuInativa_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.EditarAsync(It.IsAny<int>(), It.IsAny<RacaRequestDto>()))
                .ReturnsAsync(OperationResult<RacaEntity?>.Failure("Raça não encontrada", 404));

            // Act
            var response = await _client.PutAsJsonAsync("/api/racas/999",
                new RacaRequestDto { Raca = "X", IdEspecie = 1 });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Racas")]
        public async Task Delete_QuandoInativadaComSucesso_DeveRetornar200()
        {
            // Arrange
            _useCase.Setup(x => x.InativarAsync(3))
                .ReturnsAsync(OperationResult<RacaEntity?>.Success(new RacaEntity { Id = 3, Raca = "Bulldog", IdEspecie = 1, StAtivo = "N" }));

            // Act
            var response = await _client.DeleteAsync("/api/racas/3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
