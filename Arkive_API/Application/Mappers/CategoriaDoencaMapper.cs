using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

namespace Arkive_API.Application.Mappers
{
    public static class CategoriaDoencaMapper
    {
        public static CategoriaDoencaEntity ToCategoriaDoencaEntity(this CategoriaDoencaRequestDto obj)
        {
            return new CategoriaDoencaEntity
            {
                Nome = obj.Nome
            };
        }
    }
}
