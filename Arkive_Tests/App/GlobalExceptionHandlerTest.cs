using System.Net;
using Arkive_Application.Exceptions;
using Arkive_Application.Interfaces;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class GlobalExceptionHandlerTest
    {
        private readonly Mock<IEspecieUseCase> _useCase;
        private readonly HttpClient _client;

        public GlobalExceptionHandlerTest(CustomWebApplicationFactory factory)
        {
            _useCase = factory.EspecieUseCaseMock;
            _useCase.Reset();
            _client = factory.CreateAuthenticatedClient();
        }

        [Fact]
        [Trait("Handler", "GlobalException")]
        public async Task ExcecaoInesperada_DeveRetornar500ProblemDetailsSemVazarDetalhes()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("senha-do-banco-vazada"));

            // Act
            var response = await _client.GetAsync("/api/especies");
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
            Assert.DoesNotContain("senha-do-banco-vazada", body);
            Assert.DoesNotContain("   at ", body);
        }

        [Fact]
        [Trait("Handler", "GlobalException")]
        public async Task NaoEncontradaException_DeveRetornar404ComAMensagem()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new EspecieNaoEncontradaException(9));

            // Act
            var response = await _client.GetAsync("/api/especies");
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
            Assert.Contains("Espécie com ID 9 não encontrada.", body.Replace("\\u00E9", "é").Replace("\\u00E7", "ç").Replace("\\u00E3", "ã"));
        }

        [Fact]
        [Trait("Handler", "GlobalException")]
        public async Task ArgumentOutOfRangeException_DeveRetornar400()
        {
            // Arrange
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentOutOfRangeException("nota"));

            // Act
            var response = await _client.GetAsync("/api/especies");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
