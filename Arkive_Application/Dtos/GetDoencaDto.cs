using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Dtos
{
    public class GetDoencaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? IdCategoria { get; set; }
        public CategoriaDoencaEntity? Categoria { get; set; }
        public string? Descricao { get; set; }
        public string? CID { get; set; }
        public string? Sintomas { get; set; }
        public string StAtivo { get; set; } = string.Empty;

        public List<LinkDto> Links { get; set; } = new();
    }
}
