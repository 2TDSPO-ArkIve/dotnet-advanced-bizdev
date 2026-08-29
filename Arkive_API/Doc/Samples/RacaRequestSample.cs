using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class RacaRequestSample : IExamplesProvider<RacaRequestDto>
    {
        public RacaRequestDto GetExamples()
        {
            return new RacaRequestDto
            {
                Raca = "Labrador Retriever",
                IdEspecie = 1,
                Porte = "GRANDE"
            };
        }
    }
}
