using System.Net;
using System.Net.Http.Json;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class AuthControllerTest
    {
        private readonly Mock<IAuthUseCase> _useCase;
        private readonly HttpClient _client;

        public AuthControllerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.AuthUseCaseMock;
            _useCase.Reset();
            // Login é público: cliente sem token
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "Auth")]
        public async Task Login_CredenciaisValidas_DeveRetornar200ComToken()
        {
            // Arrange
            _useCase.Setup(x => x.AutenticarAsync(It.IsAny<PostAuthDto>()))
                .ReturnsAsync(OperationResult<GetAuthDto?>.Success(new GetAuthDto { Login = "admin@arkive.com", Jwt = "token" }));

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth", new PostAuthDto { Login = "admin@arkive.com", Senha = "Senha@123" });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<GetAuthDto>();
            Assert.Equal("token", body!.Jwt);
        }

        [Fact]
        [Trait("Controller", "Auth")]
        public async Task Login_CredenciaisInvalidas_DeveRetornar401()
        {
            // Arrange
            _useCase.Setup(x => x.AutenticarAsync(It.IsAny<PostAuthDto>()))
                .ReturnsAsync(OperationResult<GetAuthDto?>.Failure("Credenciais inválidas", 401));

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth", new PostAuthDto { Login = "admin@arkive.com", Senha = "errada" });

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
