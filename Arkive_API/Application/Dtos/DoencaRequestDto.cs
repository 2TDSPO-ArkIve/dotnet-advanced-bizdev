using System.ComponentModel.DataAnnotations;

namespace Arkive_API.Application.Dtos
{
    public class DoencaRequestDto
    {
        [Required(ErrorMessage = "O nome da Doença é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome da Doença deve ter entre 1 e 100 caracteres.")]
        public string Nome { get; set; }

        public int? IdCategoria { get; set; }

        public string? Descricao { get; set; }

        [StringLength(20, ErrorMessage = "O CID deve ter no máximo 20 caracteres.")]
        public string? CID { get; set; }

        public string? Sintomas { get; set; }
    }
}
