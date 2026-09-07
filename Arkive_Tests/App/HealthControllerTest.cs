using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Arkive_Tests.App
{
    public class HealthControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public HealthControllerTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        [Trait("Controller", "Health")]
        public async Task Live_DeveRetornar200_QuandoApiNoAr()
        {
            // Arrange / Act
            var response = await _client.GetAsync("/health/live");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Health")]
        public async Task LiveDetalhado_DeveRetornarStatusHealthy()
        {
            // Arrange / Act
            var response = await _client.GetAsync("/api/health2/live");
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Healthy", body);
        }
    }
}
