using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class CategoriaDoencaResponseSample : IExamplesProvider<CategoriaDoencaEntity>
    {
        public CategoriaDoencaEntity GetExamples()
        {
            return new CategoriaDoencaEntity
            {
                Id = 1,
                Nome = "Doenças Ortopédicas",
                StAtivo = "S"
            };
        }
    }

    public class CategoriaDoencaByIdResponseSample : IExamplesProvider<GetCategoriaDoencaDto>
    {
        public GetCategoriaDoencaDto GetExamples()
        {
            return new CategoriaDoencaResponseSample().GetExamples().ToCategoriaDoencaDto(Pagination.ItemLinks("/api/categorias-doenca", 1, "categoria_doenca"));
        }
    }
}
