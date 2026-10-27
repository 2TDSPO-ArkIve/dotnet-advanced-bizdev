using Arkive_Application.Dtos;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;

namespace Arkive_Application.Mappers
{
    public static class EspecieMapper
    {
        public static EspecieEntity ToEspecieEntity(this EspecieRequestDto obj)
        {
            return new EspecieEntity
            {
                Especie = obj.Especie
            };
        }

        public static GetEspecieDto ToEspecieDto(this EspecieEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetEspecieDto
            {
                Id = obj.Id,
                Especie = obj.Especie,
                StAtivo = obj.StAtivo,
                Links = links.ToList()
            };
        }
    }
}
