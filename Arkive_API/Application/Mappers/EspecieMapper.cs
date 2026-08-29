using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Mappers
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
    }
}
