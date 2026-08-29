using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class CategoriaDoencaRequestSample : IExamplesProvider<CategoriaDoencaRequestDto>
    {
        public CategoriaDoencaRequestDto GetExamples()
        {
            return new CategoriaDoencaRequestDto
            {
                Nome = "Doenças Ortopédicas"
            };
        }
    }
}
