using System.ComponentModel.DataAnnotations;

namespace Arkive_API.Application.Dtos
{
    public class FeedbackNPSRequestDto
    {
        public int? IdResponsavel { get; set; }

        public int? IdAnimal { get; set; }

        public int? IdClinica { get; set; }

        public int? IdConsulta { get; set; }

        public int? IdVeterinario { get; set; }

        [Required(ErrorMessage = "A Nota é obrigatória.")]
        [Range(0, 10, ErrorMessage = "A Nota deve estar entre 0 e 10")]
        public int Nota { get; set; }

        public string? Comentario { get; set; }
    }
}
