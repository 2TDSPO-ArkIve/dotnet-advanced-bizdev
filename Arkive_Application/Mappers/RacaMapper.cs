using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Mappers
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

        public static GetRacaDto ToRacaDto(this RacaEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetRacaDto
            {
                Id = obj.Id,
                Raca = obj.Raca,
                IdEspecie = obj.IdEspecie,
                Especie = obj.Especie,
                Porte = obj.Porte,
                StAtivo = obj.StAtivo,
                Links = links.ToList()
            };
        }
    }
}
