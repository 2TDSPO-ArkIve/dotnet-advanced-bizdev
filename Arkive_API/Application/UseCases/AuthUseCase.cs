using Arkive_API.Application.Dtos;
using Arkive_API.Application.Interfaces;
using Arkive_API.Domain.Interfaces;
using Arkive_API.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Arkive_API.Application.UseCases
{
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthUseCase> _logger;

        public AuthUseCase(IUsuarioRepository usuarioRepository, IConfiguration configuration, ILogger<AuthUseCase> logger)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<OperationResult<GetAuthDto?>> AutenticarAsync(PostAuthDto dto)
        {
            _logger.LogInformation("Autenticando usuário {Login}", dto.Login);

            try
            {
                var usuario = await _usuarioRepository.ObterPorLoginAsync(dto.Login);

                // Mesma resposta para login inexistente, inativo ou senha errada: não revela se o login existe
                if (usuario is null || usuario.StAtivo != "S" || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                {
                    _logger.LogWarning("Falha de autenticação para {Login}", dto.Login);
                    return OperationResult<GetAuthDto?>.Failure("Credenciais inválidas", 401);
                }

                var jwt = GerarToken(usuario.Login, usuario.TipoUsuario, _configuration["Secretkey"]!);

                return OperationResult<GetAuthDto?>.Success(new GetAuthDto { Login = usuario.Login, Jwt = jwt });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao autenticar usuário {Login}", dto.Login);
                return OperationResult<GetAuthDto?>.Failure("Ocorreu um erro ao autenticar o usuário");
            }
        }

        /// <summary>
        /// Gera o JWT (HMAC-SHA256, 8h) com as claims Name = login e Role = perfil (TP_USUARIO).
        /// </summary>
        public static string GerarToken(string login, string role, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, login),
                    new Claim(ClaimTypes.Role, role),
                })
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
