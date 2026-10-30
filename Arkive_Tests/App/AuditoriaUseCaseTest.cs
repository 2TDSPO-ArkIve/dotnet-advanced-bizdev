using Arkive_Application.UseCases;
using Arkive_Domain.Entities;
using Arkive_Domain.Interfaces;
using Arkive_Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Bson;
using Moq;

namespace Arkive_Tests.App
{
    public class AuditoriaUseCaseTest
    {
        private readonly Mock<IAuditoriaRepository> _repository = new();
        private readonly AuditoriaUseCase _useCase;

        public AuditoriaUseCaseTest()
        {
            _useCase = new AuditoriaUseCase(_repository.Object, NullLogger<AuditoriaUseCase>.Instance);
        }

        [Fact]
        [Trait("UseCase", "Auditoria")]
        public async Task ObterTodasAsync_DeveMapearItensEMontarLinks()
        {
            // Arrange
            var id = ObjectId.GenerateNewId();
            _repository.Setup(x => x.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PageResultModel<IEnumerable<AuditoriaEntity>>
            {
                Data = new List<AuditoriaEntity> { new() { Id = id, Usuario = "admin", Metodo = "POST", Rota = "/api/auth", StatusCode = 200, Requisicao = "{\"a\":1}", Resposta = "{\"b\":2}" } },
                TotalRegistros = 1
            });

            // Act
            var resultado = await _useCase.ObterTodasAsync();

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(id.ToString(), resultado.Value!.Data.Single().Id);
            Assert.Equal("{\"a\":1}", resultado.Value.Data.Single().Requisicao);
            Assert.Equal("{\"b\":2}", resultado.Value.Data.Single().Resposta);
            Assert.Contains(resultado.Value.Links, l => l.Rel == "self");
        }

        [Fact]
        [Trait("UseCase", "Auditoria")]
        public async Task ObterTodasAsync_SemRegistros_DeveRetornar204()
        {
            _repository.Setup(x => x.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PageResultModel<IEnumerable<AuditoriaEntity>> { Data = new List<AuditoriaEntity>() });

            var resultado = await _useCase.ObterTodasAsync();

            Assert.False(resultado.IsSuccess);
            Assert.Equal(204, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Auditoria")]
        public async Task ObterTodasAsync_ComExcecao_DeveRetornar400SemVazarMensagem()
        {
            _repository.Setup(x => x.ObterTodosAsync(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception("detalhe interno"));

            var resultado = await _useCase.ObterTodasAsync();

            Assert.Equal(400, resultado.StatusCode);
            Assert.DoesNotContain("detalhe interno", resultado.Error);
        }
    }
}
