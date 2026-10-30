using Arkive_Application.Dtos;
using Arkive_Application.UseCases;
using Arkive_Domain.Entities.External;
using Arkive_Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace Arkive_Tests.App
{
    public class AuthUseCaseTest
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepository;
        private readonly AuthUseCase _authUseCase;

        public AuthUseCaseTest()
        {
            _usuarioRepository = new Mock<IUsuarioRepository>();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["Secretkey"] = CustomWebApplicationFactory.SecretkeyTeste })
                .Build();

            // Isso é o que vamos testar
            _authUseCase = new AuthUseCase(_usuarioRepository.Object, configuration, NullLogger<AuthUseCase>.Instance);
        }

        // workFactor 4 (mínimo) só para o teste ser rápido; o Java gera com o padrão 10
        private static UsuarioExternal Usuario(string stAtivo = "S") => new UsuarioExternal
        {
            Id = 1,
            Nome = "Admin",
            Login = "admin@arkive.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("Senha@123", 4),
            TipoUsuario = "SYSADMIN",
            StAtivo = stAtivo
        };

        [Fact]
        [Trait("UseCase", "Auth")]
        public async Task AutenticarAsync_CredenciaisValidas_DeveRetornarTokenComClaims()
        {
            // Arrange
            _usuarioRepository.Setup(obj => obj.ObterPorLoginAsync("admin@arkive.com")).ReturnsAsync(Usuario());

            // Act
            var resultado = await _authUseCase.AutenticarAsync(new PostAuthDto { Login = "admin@arkive.com", Senha = "Senha@123" });

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal("admin@arkive.com", resultado.Value!.Login);

            var token = new JwtSecurityTokenHandler().ReadJwtToken(resultado.Value.Jwt);
            Assert.Equal("admin@arkive.com", token.Claims.First(c => c.Type == "unique_name").Value);
            Assert.Equal("SYSADMIN", token.Claims.First(c => c.Type == "role").Value);
        }

        [Fact]
        [Trait("UseCase", "Auth")]
        public async Task AutenticarAsync_SenhaErrada_DeveRetornar401()
        {
            // Arrange
            _usuarioRepository.Setup(obj => obj.ObterPorLoginAsync("admin@arkive.com")).ReturnsAsync(Usuario());

            // Act
            var resultado = await _authUseCase.AutenticarAsync(new PostAuthDto { Login = "admin@arkive.com", Senha = "errada" });

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(401, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Auth")]
        public async Task AutenticarAsync_LoginInexistente_DeveRetornar401()
        {
            // Arrange
            _usuarioRepository.Setup(obj => obj.ObterPorLoginAsync(It.IsAny<string>())).ReturnsAsync((UsuarioExternal?)null);

            // Act
            var resultado = await _authUseCase.AutenticarAsync(new PostAuthDto { Login = "ninguem@arkive.com", Senha = "Senha@123" });

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(401, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Auth")]
        public async Task AutenticarAsync_UsuarioInativo_DeveRetornar401()
        {
            // Arrange
            _usuarioRepository.Setup(obj => obj.ObterPorLoginAsync("admin@arkive.com")).ReturnsAsync(Usuario(stAtivo: "N"));

            // Act
            var resultado = await _authUseCase.AutenticarAsync(new PostAuthDto { Login = "admin@arkive.com", Senha = "Senha@123" });

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(401, resultado.StatusCode);
        }

        [Fact]
        [Trait("UseCase", "Auth")]
        public async Task AutenticarAsync_ErroNoRepositorio_DeveRetornar400ComMensagemGenerica()
        {
            // Arrange
            _usuarioRepository.Setup(obj => obj.ObterPorLoginAsync(It.IsAny<string>())).ThrowsAsync(new Exception("ORA-12541"));

            // Act
            var resultado = await _authUseCase.AutenticarAsync(new PostAuthDto { Login = "admin@arkive.com", Senha = "Senha@123" });

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(400, resultado.StatusCode);
            Assert.Equal("Ocorreu um erro ao autenticar o usuário", resultado.Error);
        }
    }
}
