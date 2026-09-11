using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class FeedbackNPSRequestSample : IExamplesProvider<FeedbackNPSRequestDto>
    {
        public FeedbackNPSRequestDto GetExamples()
        {
            return new FeedbackNPSRequestDto
            {
                IdResponsavel = 1,
                IdAnimal = 1,
                IdClinica = 1,
                IdConsulta = 1,
                IdVeterinario = null,
                Nota = 5,
                Comentario = "Atendimento excelente, equipe muito atenciosa."
            };
        }
    }
}
