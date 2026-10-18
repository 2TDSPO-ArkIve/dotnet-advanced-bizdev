using Arkive_API.Application;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class RacaResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<RacaEntity>>>
    {
        public PageResultModel<IEnumerable<RacaEntity>> GetExamples()
        {
            var canina = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" };

            var data = new List<RacaEntity>
            {
                new RacaEntity { Id = 1, Raca = "Labrador Retriever", IdEspecie = 1, Especie = canina, Porte = "GRANDE", StAtivo = "S" },
                new RacaEntity { Id = 2, Raca = "Poodle", IdEspecie = 1, Especie = canina, Porte = "PEQUENO", StAtivo = "S" },
            };

            var links = Pagination.Links("/api/racas", 0, 2, 10);
            links.Add(new("/api/racas", "create_raca", "POST"));

            return new PageResultModel<IEnumerable<RacaEntity>>
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
