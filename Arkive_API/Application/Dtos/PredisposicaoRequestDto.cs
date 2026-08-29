using System.ComponentModel.DataAnnotations;

namespace Arkive_API.Application.Dtos
{
    public class PredisposicaoRequestDto
    {
        [Required(ErrorMessage = "O ID da Espécie é obrigatório.")]
        public int IdEspecie { get; set; }

        public int? IdRaca { get; set; }

        [Required(ErrorMessage = "O ID da Doença é obrigatório.")]
        public int IdDoenca { get; set; }
    }
}
