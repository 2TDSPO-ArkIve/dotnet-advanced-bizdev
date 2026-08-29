using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class EspecieResponseSample : IExamplesProvider<EspecieEntity>
    {
        public EspecieEntity GetExamples()
        {
            return new EspecieEntity
            {
                Id = 1,
                Especie = "Canina",
                StAtivo = "S"
            };
        }
    }
}
