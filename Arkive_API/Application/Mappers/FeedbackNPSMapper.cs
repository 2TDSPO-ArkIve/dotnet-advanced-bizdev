using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Mappers
{
    public static class FeedbackNPSMapper
    {
        public static FeedbackNPSEntity ToFeedbackNPSEntity(this FeedbackNPSRequestDto obj)
        {
            return new FeedbackNPSEntity
            {
                IdResponsavel = obj.IdResponsavel,
                IdAnimal = obj.IdAnimal,
                IdClinica = obj.IdClinica,
                IdConsulta = obj.IdConsulta,
                IdVeterinario = obj.IdVeterinario,
                Nota = obj.Nota,
                Comentario = obj.Comentario
                // DataFeedback não é mapeada: usa o default DateTime.Now da própria entidade.
            };
        }
    }
}
