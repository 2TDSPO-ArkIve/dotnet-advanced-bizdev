using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class EspecieRequestSample : IExamplesProvider<EspecieRequestDto>
    {
        public EspecieRequestDto GetExamples()
        {
            return new EspecieRequestDto
            {
                Especie = "Canina"
            };
        }
    }
}
