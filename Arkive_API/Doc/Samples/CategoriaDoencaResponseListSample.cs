using Arkive_API.Application;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class CategoriaDoencaResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<CategoriaDoencaEntity>>>
    {
        public PageResultModel<IEnumerable<CategoriaDoencaEntity>> GetExamples()
        {
            var data = new List<CategoriaDoencaEntity>
            {
                new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" },
                new CategoriaDoencaEntity { Id = 2, Nome = "Doenças Dermatológicas", StAtivo = "S" },
            };

            var links = Pagination.Links("/api/categorias-doenca", 0, 2, 10);
            links.Add(new("/api/categorias-doenca", "create_categoria_doenca", "POST"));

            return new PageResultModel<IEnumerable<CategoriaDoencaEntity>>
            {
                Data = data,
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = links
            };
        }
    }
}
