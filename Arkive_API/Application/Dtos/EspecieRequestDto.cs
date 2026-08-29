using System.ComponentModel.DataAnnotations;

namespace Arkive_API.Application.Dtos
{
    public class EspecieRequestDto
    {
        [Required(ErrorMessage = "O nome da Espécie é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome da Espécie deve ter no máximo 50 caracteres.")]
        public string Especie { get; set; }
    }
}
