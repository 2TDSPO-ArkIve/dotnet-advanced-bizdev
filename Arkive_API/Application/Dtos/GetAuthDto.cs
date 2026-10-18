namespace Arkive_API.Application.Dtos
{
    public class GetAuthDto
    {
        public string Login { get; set; } = string.Empty;
        public string Jwt { get; set; } = string.Empty;
    }
}
