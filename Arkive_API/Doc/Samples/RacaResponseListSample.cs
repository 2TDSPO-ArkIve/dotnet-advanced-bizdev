using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Mappers;
using Arkive_Domain.Entities;
using Arkive_Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class RacaResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<GetRacaDto>>>
    {
        public PageResultModel<IEnumerable<GetRacaDto>> GetExamples()
        {
            var canina = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" };

            var data = new List<RacaEntity>
            {
                new RacaEntity { Id = 1, Raca = "Labrador Retriever", IdEspecie = 1, Especie = canina, Porte = "GRANDE", StAtivo = "S" },
                new RacaEntity { Id = 2, Raca = "Poodle", IdEspecie = 1, Especie = canina, Porte = "PEQUENO", StAtivo = "S" },
            };

            var links = Pagination.Links("/api/racas", 0, 2, 10);
            links.Add(new("/api/racas", "create_raca", "POST"));

            return new PageResultModel<IEnumerable<GetRacaDto>>
            {
                Data = data.Select(x => x.ToRacaDto(Pagination.ItemLinks("/api/racas", x.Id, "raca", ativo: x.StAtivo == "S", naLista: true))).ToList(),
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = links
            };
        }
    }
}
