using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Dtos
{
    public class GetCategoriaDoencaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string StAtivo { get; set; } = string.Empty;

        public List<LinkDto> Links { get; set; } = new();
    }
}
