using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;

namespace Arkive_API.Application.Mappers
{
    public static class DoencaMapper
    {
        public static DoencaEntity ToDoencaEntity(this DoencaRequestDto obj)
        {
            return new DoencaEntity
            {
                Nome = obj.Nome,
                IdCategoria = obj.IdCategoria,
                Descricao = obj.Descricao,
                CID = obj.CID,
                Sintomas = obj.Sintomas
            };
        }

        public static GetDoencaDto ToDoencaDto(this DoencaEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetDoencaDto
            {
                Id = obj.Id,
                Nome = obj.Nome,
                IdCategoria = obj.IdCategoria,
                Categoria = obj.Categoria,
                Descricao = obj.Descricao,
                CID = obj.CID,
                Sintomas = obj.Sintomas,
                StAtivo = obj.StAtivo,
                Links = links.ToList()
            };
        }
    }
}
