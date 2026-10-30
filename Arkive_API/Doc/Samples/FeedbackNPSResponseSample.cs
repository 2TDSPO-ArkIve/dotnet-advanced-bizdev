using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Mappers;
using Arkive_Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class FeedbackNPSResponseSample : IExamplesProvider<FeedbackNPSEntity>
    {
        public FeedbackNPSEntity GetExamples()
        {
            return new FeedbackNPSEntity
            {
                Id = 1,
                IdResponsavel = 1,
                IdAnimal = 1,
                IdClinica = 1,
                IdConsulta = 1,
                IdVeterinario = null,
                Nota = 5,
                Comentario = "Atendimento excelente, equipe muito atenciosa.",
                DataFeedback = new DateTime(2026, 8, 20, 14, 30, 0)
            };
        }
    }

    public class FeedbackNPSByIdResponseSample : IExamplesProvider<GetFeedbackNPSDto>
    {
        public GetFeedbackNPSDto GetExamples()
        {
            return new FeedbackNPSResponseSample().GetExamples().ToFeedbackNPSDto(Pagination.ItemLinks("/api/feedbacks-nps", 1, "feedback_nps", editavel: false));
        }
    }
}
