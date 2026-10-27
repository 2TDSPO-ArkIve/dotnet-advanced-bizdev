using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Mappers;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class FeedbackNPSResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<GetFeedbackNPSDto>>>
    {
        public PageResultModel<IEnumerable<GetFeedbackNPSDto>> GetExamples()
        {
            var data = new List<FeedbackNPSEntity>
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

            var links = Pagination.Links("/api/feedbacks-nps", 0, 2, 10);
            links.Add(new("/api/feedbacks-nps", "create_feedback_nps", "POST"));

            return new PageResultModel<IEnumerable<GetFeedbackNPSDto>>
            {
                Data = data.Select(x => x.ToFeedbackNPSDto(Pagination.ItemLinks("/api/feedbacks-nps", x.Id, "feedback_nps", editavel: false, naLista: true))).ToList(),
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = links
            };
        }
    }
}
