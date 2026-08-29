using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Mappers
{
    public static class RacaMapper
    {
        public static RacaEntity ToRacaEntity(this RacaRequestDto obj)
        {
            return new RacaEntity
            {
                Raca = obj.Raca,
                IdEspecie = obj.IdEspecie,
                Porte = obj.Porte?.ToUpper()
            };
        }
    }
}
