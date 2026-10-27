using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Mappers
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

        public static GetPredisposicaoDto ToPredisposicaoDto(this PredisposicaoEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetPredisposicaoDto
            {
                Id = obj.Id,
                IdEspecie = obj.IdEspecie,
                Especie = obj.Especie,
                IdRaca = obj.IdRaca,
                Raca = obj.Raca,
                IdDoenca = obj.IdDoenca,
                Doenca = obj.Doenca,
                Links = links.ToList()
            };
        }
    }
}
