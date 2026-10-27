using System.ComponentModel.DataAnnotations;

namespace Arkive_Application.Dtos
{
    public class CategoriaDoencaRequestDto
    {
        [Required(ErrorMessage = "O nome da Categoria é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome da Categoria deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
    }
}
