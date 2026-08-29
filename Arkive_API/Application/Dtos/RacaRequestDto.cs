using System.ComponentModel.DataAnnotations;

namespace Arkive_API.Application.Dtos
{
    public class RacaRequestDto
    {
        [Required(ErrorMessage = "O nome da Raça é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome da Raça deve ter no máximo 50 caracteres.")]
        public string Raca { get; set; }

        [Required(ErrorMessage = "O ID da Espécie é obrigatório.")]
        public int IdEspecie { get; set; }

        [RegularExpression("(?i)^(PEQUENO|MEDIO|GRANDE)$", ErrorMessage = "Tamanho/Porte inválido.")]
        public string? Porte { get; set; }
    }
}
