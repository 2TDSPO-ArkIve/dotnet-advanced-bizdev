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
}
