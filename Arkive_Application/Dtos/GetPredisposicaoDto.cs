using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Dtos
{
    public class GetPredisposicaoDto
    {
        public int Id { get; set; }
        public int IdEspecie { get; set; }
        public EspecieEntity? Especie { get; set; }
        public int? IdRaca { get; set; }
        public RacaEntity? Raca { get; set; }
        public int IdDoenca { get; set; }
        public DoencaEntity? Doenca { get; set; }

        public List<LinkDto> Links { get; set; } = new();
    }
}
