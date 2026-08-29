using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class DoencaResponseListSample : IExamplesProvider<IEnumerable<DoencaEntity>>
    {
        public IEnumerable<DoencaEntity> GetExamples()
        {
            var categoria = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" };

            return new List<DoencaEntity>
            {
                new DoencaEntity
                {
                    Id = 1,
                    Nome = "Displasia Coxofemoral",
                    IdCategoria = 1,
                    Categoria = categoria,
                    CID = "M16",
                    StAtivo = "S"
                },
                new DoencaEntity
                {
                    Id = 2,
                    Nome = "Luxação de Patela",
                    IdCategoria = 1,
                    Categoria = categoria,
                    CID = "M22",
                    StAtivo = "S"
                },
            };
        }
    }
}
