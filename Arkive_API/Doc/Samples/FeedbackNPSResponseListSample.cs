using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class FeedbackNPSResponseListSample : IExamplesProvider<IEnumerable<FeedbackNPSEntity>>
    {
        public IEnumerable<FeedbackNPSEntity> GetExamples()
        {
            return new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity
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
                },
                new FeedbackNPSEntity
                {
                    Id = 2,
                    IdResponsavel = null,
                    IdAnimal = null,
                    IdClinica = 2,
                    IdConsulta = null,
                    IdVeterinario = 3,
                    Nota = 3,
                    Comentario = "Tempo de espera um pouco longo.",
                    DataFeedback = new DateTime(2026, 8, 22, 9, 15, 0)
                },
            };
        }
    }
}
