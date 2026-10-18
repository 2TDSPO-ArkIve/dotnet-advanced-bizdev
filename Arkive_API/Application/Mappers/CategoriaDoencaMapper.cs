using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

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

        public static GetCategoriaDoencaDto ToCategoriaDoencaDto(this CategoriaDoencaEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetCategoriaDoencaDto
            {
                Id = obj.Id,
                Nome = obj.Nome,
                StAtivo = obj.StAtivo,
                Links = links.ToList()
            };
        }
    }
}
