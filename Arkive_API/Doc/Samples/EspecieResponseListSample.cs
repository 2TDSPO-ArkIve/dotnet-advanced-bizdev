using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class EspecieResponseListSample : IExamplesProvider<IEnumerable<EspecieEntity>>
    {
        public IEnumerable<EspecieEntity> GetExamples()
        {
            return new List<EspecieEntity>
            {
                new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "S" },
            };
        }
    }
}
