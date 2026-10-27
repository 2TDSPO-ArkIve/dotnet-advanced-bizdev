using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Dtos
{
    public class GetRacaDto
    {
        public int Id { get; set; }
        public string Raca { get; set; } = string.Empty;
        public int IdEspecie { get; set; }
        public EspecieEntity? Especie { get; set; }
        public string? Porte { get; set; }
        public string StAtivo { get; set; } = string.Empty;

        public List<LinkDto> Links { get; set; } = new();
    }
}
