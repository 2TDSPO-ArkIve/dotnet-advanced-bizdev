using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Entities;

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
    }
}
