using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class PredisposicaoRequestSample : IExamplesProvider<PredisposicaoRequestDto>
    {
        public PredisposicaoRequestDto GetExamples()
        {
            return new PredisposicaoRequestDto
            {
                IdEspecie = 1,
                IdRaca = 1,
                IdDoenca = 1
            };
        }
    }
}
