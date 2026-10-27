using Arkive_API.Application.Interfaces;
using Arkive_API.Application.UseCases;
using Arkive_API.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net.Http.Headers;

namespace Arkive_Tests.App
{
    /// <summary>
    /// Sobe a API real em memória (sem servidor, sem banco Oracle) para os testes funcionais
    /// de Controller. Cada I*UseCase é substituído por um Mock, então o teste exercita
    /// Controller + pipeline HTTP (rotas, status codes, serialização) sem tocar Application/Infra.
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public const string SecretkeyTeste = "chave-de-teste-arkive-com-mais-de-32-caracteres";

        public Mock<ICategoriaDoencaUseCase> CategoriaDoencaUseCaseMock { get; } = new();
        public Mock<IEspecieUseCase> EspecieUseCaseMock { get; } = new();
        public Mock<IDoencaUseCase> DoencaUseCaseMock { get; } = new();
        public Mock<IRacaUseCase> RacaUseCaseMock { get; } = new();
        public Mock<IPredisposicaoUseCase> PredisposicaoUseCaseMock { get; } = new();
        public Mock<IFeedbackNPSUseCase> FeedbackNPSUseCaseMock { get; } = new();
        public Mock<IAuthUseCase> AuthUseCaseMock { get; } = new();
        public Mock<IAuditoriaUseCase> AuditoriaUseCaseMock { get; } = new();
        public Mock<IAuditoriaRepository> AuditoriaRepositoryMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            // appsettings.Development.json não carrega em "Testing"; sem a chave a API não sobe
            builder.UseSetting("Secretkey", SecretkeyTeste);

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(ICategoriaDoencaUseCase));
                services.RemoveAll(typeof(IEspecieUseCase));
                services.RemoveAll(typeof(IDoencaUseCase));
                services.RemoveAll(typeof(IRacaUseCase));
                services.RemoveAll(typeof(IPredisposicaoUseCase));
                services.RemoveAll(typeof(IFeedbackNPSUseCase));
                services.RemoveAll(typeof(IAuthUseCase));
                services.RemoveAll(typeof(IAuditoriaUseCase));
                services.RemoveAll(typeof(IAuditoriaRepository));

                services.AddSingleton(CategoriaDoencaUseCaseMock.Object);
                services.AddSingleton(EspecieUseCaseMock.Object);
                services.AddSingleton(DoencaUseCaseMock.Object);
                services.AddSingleton(RacaUseCaseMock.Object);
                services.AddSingleton(PredisposicaoUseCaseMock.Object);
                services.AddSingleton(FeedbackNPSUseCaseMock.Object);
                services.AddSingleton(AuthUseCaseMock.Object);
                services.AddSingleton(AuditoriaUseCaseMock.Object);
                services.AddSingleton(AuditoriaRepositoryMock.Object);
            });
        }

        /// <summary>
        /// Cliente HTTP já com "Authorization: Bearer" assinado pela chave de teste, no perfil informado.
        /// </summary>
        public HttpClient CreateAuthenticatedClient(string role = "SYSADMIN")
        {
            var client = CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                AuthUseCase.GerarToken("teste@arkive.com", role, SecretkeyTeste));
            return client;
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
