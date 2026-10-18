using Arkive_API.Application;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class EspecieResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<EspecieEntity>>>
    {
        public PageResultModel<IEnumerable<EspecieEntity>> GetExamples()
        {
            var data = new List<EspecieEntity>
            {
                new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "S" },
            };

            var links = Pagination.Links("/api/especies", 0, 2, 10);
            links.Add(new("/api/especies", "create_especie", "POST"));

            return new PageResultModel<IEnumerable<EspecieEntity>>
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
