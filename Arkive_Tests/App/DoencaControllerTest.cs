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
    public class DoencaControllerTest
    {
        private readonly Mock<IDoencaUseCase> _useCase;
        private readonly HttpClient _client;

        public DoencaControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.DoencaUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetAll_ComListaDeDoencas_DeveRetornar200()
        {
            // Arrange
            var doencas = new List<DoencaEntity>
            {
                new DoencaEntity { Id = 1, Nome = "Cinomose", StAtivo = "S" },
                new DoencaEntity { Id = 2, Nome = "Raiva", StAtivo = "N" }
            };
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<DoencaEntity>> { Data = doencas, TotalRegistros = doencas.Count });

            // Act
            var response = await _client.GetAsync("/api/doencas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<DoencaEntity>>>();
            Assert.Equal(2, body!.Data.Count);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetAll_QuandoNaoHaDoencas_DeveRetornar204()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new PageResultModel<IEnumerable<DoencaEntity>> { Data = new List<DoencaEntity>() });

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
                .ReturnsAsync(new DoencaEntity { Id = 7, Nome = "Parvovirose", StAtivo = "S" });

            // Act
            var response = await _client.GetAsync("/api/doencas/7");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<DoencaEntity>();
            Assert.Equal("Parvovirose", body!.Nome);
        }

        [Fact]
        [Trait("Controller", "Doencas")]
        public async Task GetById_QuandoDoencaNaoExiste_DeveRetornar404()
        {
            // Arrange
            _useCase.Setup(x => x.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((DoencaEntity?)null);

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
                .ReturnsAsync(new DoencaEntity { Id = 20, Nome = "Leptospirose", StAtivo = "S" });

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
                .ThrowsAsync(new CategoriaNaoEncontradaException(999));

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
                .ReturnsAsync((DoencaEntity?)null);

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
                .ReturnsAsync(new DoencaEntity { Id = 3, Nome = "Cinomose", StAtivo = "N" });

            // Act
            var response = await _client.DeleteAsync("/api/doencas/3");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
