using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Mappers
{
    public static class PredisposicaoMapper
    {
        public static PredisposicaoEntity ToPredisposicaoEntity(this PredisposicaoRequestDto obj)
        {
            return new PredisposicaoEntity
            {
                IdEspecie = obj.IdEspecie,
                IdRaca = obj.IdRaca,
                IdDoenca = obj.IdDoenca
            };
        }
    }
}
