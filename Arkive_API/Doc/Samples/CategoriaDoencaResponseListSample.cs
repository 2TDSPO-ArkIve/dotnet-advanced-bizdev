using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class CategoriaDoencaResponseListSample : IExamplesProvider<IEnumerable<CategoriaDoencaEntity>>
    {
        public IEnumerable<CategoriaDoencaEntity> GetExamples()
        {
            return new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" },
                new CategoriaDoencaEntity { Id = 2, Nome = "Doenças Dermatológicas", StAtivo = "S" },
            };
        }
    }
}
