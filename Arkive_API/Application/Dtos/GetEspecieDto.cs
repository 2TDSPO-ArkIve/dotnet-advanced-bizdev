using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Dtos
{
    public class GetEspecieDto
    {
        public int Id { get; set; }
        public string Especie { get; set; } = string.Empty;
        public string StAtivo { get; set; } = string.Empty;

        public List<LinkDto> Links { get; set; } = new();
    }
}
