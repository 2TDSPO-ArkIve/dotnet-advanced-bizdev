using Arkive_API.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Arkive_Tests.App
{
    /// <summary>
    /// Sobe a API real em memória (sem servidor, sem banco Oracle) para os testes funcionais
    /// de Controller. Cada I*UseCase é substituído por um Mock, então o teste exercita
    /// Controller + pipeline HTTP (rotas, status codes, serialização) sem tocar Application/Infra.
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<ICategoriaDoencaUseCase> CategoriaDoencaUseCaseMock { get; } = new();
        public Mock<IEspecieUseCase> EspecieUseCaseMock { get; } = new();
        public Mock<IDoencaUseCase> DoencaUseCaseMock { get; } = new();
        public Mock<IRacaUseCase> RacaUseCaseMock { get; } = new();
        public Mock<IPredisposicaoUseCase> PredisposicaoUseCaseMock { get; } = new();
        public Mock<IFeedbackNPSUseCase> FeedbackNPSUseCaseMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(ICategoriaDoencaUseCase));
                services.RemoveAll(typeof(IEspecieUseCase));
                services.RemoveAll(typeof(IDoencaUseCase));
                services.RemoveAll(typeof(IRacaUseCase));
                services.RemoveAll(typeof(IPredisposicaoUseCase));
                services.RemoveAll(typeof(IFeedbackNPSUseCase));

                services.AddSingleton(CategoriaDoencaUseCaseMock.Object);
                services.AddSingleton(EspecieUseCaseMock.Object);
                services.AddSingleton(DoencaUseCaseMock.Object);
                services.AddSingleton(RacaUseCaseMock.Object);
                services.AddSingleton(PredisposicaoUseCaseMock.Object);
                services.AddSingleton(FeedbackNPSUseCaseMock.Object);
            });
        }
    }

    /// <summary>
    /// Collection Fixture: compartilha UMA única instância de <see cref="CustomWebApplicationFactory"/>
    /// entre todas as classes de teste de Controller marcadas com [Collection("Controller Collection")],
    /// em vez de criar uma instância por classe (o que IClassFixture faria sozinho).
    /// </summary>
    [CollectionDefinition("Controller Collection")]
    public class ControllerCollection : ICollectionFixture<CustomWebApplicationFactory> { }
}
