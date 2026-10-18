using Arkive_API.Domain.Entities.External;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class UsuarioRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsuarioTestDatabase")
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _usuarioRepository = new UsuarioRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Usuarios")]
        public async Task ObterPorLoginAsync_LoginExistente_DeveRetornarUsuario()
        {
            // Arrange
            _applicationContext.Usuario.AddRange(
                new UsuarioExternal { Nome = "Admin", Login = "admin@arkive.com", SenhaHash = "hash", TipoUsuario = "SYSADMIN" },
                new UsuarioExternal { Nome = "Vet", Login = "vet@arkive.com", SenhaHash = "hash", TipoUsuario = "VETERINARIO" });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _usuarioRepository.ObterPorLoginAsync("vet@arkive.com");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Vet", resultado.Nome);
            Assert.Equal("VETERINARIO", resultado.TipoUsuario);
        }

        [Fact]
        [Trait("Repository", "Usuarios")]
        public async Task ObterPorLoginAsync_LoginInexistente_DeveRetornarNull()
        {
            // Arrange
            _applicationContext.Usuario.Add(new UsuarioExternal { Nome = "Admin", Login = "admin@arkive.com", SenhaHash = "hash", TipoUsuario = "SYSADMIN" });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _usuarioRepository.ObterPorLoginAsync("ninguem@arkive.com");

            // Assert
            Assert.Null(resultado);
        }
    }
}
