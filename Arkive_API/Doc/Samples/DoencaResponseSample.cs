using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Mappers;
using Arkive_Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class DoencaResponseSample : IExamplesProvider<DoencaEntity>
    {
        public DoencaEntity GetExamples()
        {
            return new DoencaEntity
            {
                Id = 1,
                Nome = "Displasia Coxofemoral",
                IdCategoria = 1,
                Categoria = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" },
                Descricao = "Malformação da articulação do quadril, comum em raças de grande porte.",
                CID = "M16",
                Sintomas = "Dificuldade para subir escadas, claudicação, dor ao levantar.",
                StAtivo = "S"
            };
        }
    }

    public class DoencaByIdResponseSample : IExamplesProvider<GetDoencaDto>
    {
        public GetDoencaDto GetExamples()
        {
            return new DoencaResponseSample().GetExamples().ToDoencaDto(Pagination.ItemLinks("/api/doencas", 1, "doenca"));
        }
    }
}
