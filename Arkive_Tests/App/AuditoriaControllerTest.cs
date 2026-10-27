using System.Net;
using System.Net.Http.Json;
using Arkive_Application.Dtos;
using Arkive_Application.Interfaces;
using Arkive_Domain.Entities;
using Arkive_Domain.Interfaces;
using Arkive_Domain.Models;
using Moq;

namespace Arkive_Tests.App
{
    [Collection("Controller Collection")]
    public class AuditoriaControllerTest
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly Mock<IAuditoriaUseCase> _useCase;
        private readonly Mock<IAuditoriaRepository> _repository;

        public AuditoriaControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _useCase = factory.AuditoriaUseCaseMock;
            _repository = factory.AuditoriaRepositoryMock;
            _useCase.Reset();
            _repository.Reset();
            factory.AuthUseCaseMock.Reset();
        }

        [Fact]
        [Trait("Controller", "Auditoria")]
        public async Task GetAll_ComSysAdmin_DeveRetornar200()
        {
            _useCase.Setup(x => x.ObterTodasAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(OperationResult<PageResultModel<IEnumerable<GetAuditoriaDto>>>.Success(new PageResultModel<IEnumerable<GetAuditoriaDto>>
            {
                Data = new List<GetAuditoriaDto> { new() { Id = "1", Usuario = "admin" } }
            }));

            var response = await _factory.CreateAuthenticatedClient().GetAsync("/api/auditoria");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<PageResultModel<List<GetAuditoriaDto>>>();
            Assert.Single(body!.Data);
        }

        [Fact]
        [Trait("Controller", "Auditoria")]
        public async Task GetAll_ComVeterinario_DeveRetornar403()
        {
            var response = await _factory.CreateAuthenticatedClient("VETERINARIO").GetAsync("/api/auditoria");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [Trait("Controller", "Auditoria")]
        public async Task GetAll_SemToken_DeveRetornar401()
        {
            var response = await _factory.CreateClient().GetAsync("/api/auditoria");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [Trait("Filter", "Auditoria")]
        public async Task Login_DeveGravarAuditoriaComLoginInformado()
        {
            _factory.AuthUseCaseMock.Setup(x => x.AutenticarAsync(It.IsAny<PostAuthDto>()))
                .ReturnsAsync(OperationResult<GetAuthDto>.Success(new GetAuthDto { Login = "vet@arkive.com", Jwt = "x" }));

            var response = await _factory.CreateClient().PostAsJsonAsync("/api/auth", new PostAuthDto { Login = "vet@arkive.com", Senha = "123" });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Sem senha na requisição e sem JWT na resposta
            _repository.Verify(x => x.AdicionarAsync(It.Is<AuditoriaEntity>(a =>
                a.Usuario == "vet@arkive.com" && a.Metodo == "POST" && a.Rota == "/api/auth" && a.StatusCode == 200
                && a.Requisicao == null && a.Resposta == null)), Times.Once);
        }

        [Fact]
        [Trait("Filter", "Auditoria")]
        public async Task PostRaca_DeveGravarDadoEnviadoEDadoRetornado()
        {
            _factory.RacaUseCaseMock.Reset();
            _factory.RacaUseCaseMock.Setup(x => x.AdicionarAsync(It.IsAny<RacaRequestDto>()))
                .ReturnsAsync(OperationResult<RacaEntity?>.Success(new RacaEntity { Id = 7, Raca = "Labrador", IdEspecie = 1, StAtivo = "S" }, 201));

            var response = await _factory.CreateAuthenticatedClient().PostAsJsonAsync("/api/racas", new { Raca = "Labrador", IdEspecie = 1 });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            _repository.Verify(x => x.AdicionarAsync(It.Is<AuditoriaEntity>(a =>
                a.Requisicao!.Contains("Labrador") && a.Resposta!.Contains("\"Id\":7"))), Times.Once);
        }

        [Fact]
        [Trait("Filter", "Auditoria")]
        public async Task Get_NaoDeveGravarAuditoria()
        {
            await _factory.CreateAuthenticatedClient().GetAsync("/api/especies");

            _repository.Verify(x => x.AdicionarAsync(It.IsAny<AuditoriaEntity>()), Times.Never);
        }

        [Fact]
        [Trait("Filter", "Auditoria")]
        public async Task Login_ComFalhaNoMongo_NaoDeveDerrubarRequisicao()
        {
            _factory.AuthUseCaseMock.Setup(x => x.AutenticarAsync(It.IsAny<PostAuthDto>()))
                .ReturnsAsync(OperationResult<GetAuthDto>.Success(new GetAuthDto { Login = "a", Jwt = "x" }));
            _repository.Setup(x => x.AdicionarAsync(It.IsAny<AuditoriaEntity>())).ThrowsAsync(new Exception("mongo fora"));

            var response = await _factory.CreateClient().PostAsJsonAsync("/api/auth", new PostAuthDto { Login = "a", Senha = "b" });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
