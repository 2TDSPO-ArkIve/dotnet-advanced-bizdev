using System.ComponentModel.DataAnnotations;

namespace Arkive_API.Application.Dtos
{
    public class PostAuthDto
    {
        [Required(ErrorMessage = "O login é obrigatório.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}
