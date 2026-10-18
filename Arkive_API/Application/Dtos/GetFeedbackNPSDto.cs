using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Dtos
{
    public class GetFeedbackNPSDto
    {
        public int Id { get; set; }
        public int? IdResponsavel { get; set; }
        public int? IdAnimal { get; set; }
        public int? IdClinica { get; set; }
        public int? IdConsulta { get; set; }
        public int? IdVeterinario { get; set; }
        public int Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataFeedback { get; set; }

        public List<LinkDto> Links { get; set; } = new();
    }
}
