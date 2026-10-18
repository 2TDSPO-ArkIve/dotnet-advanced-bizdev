using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class RacaResponseSample : IExamplesProvider<RacaEntity>
    {
        public RacaEntity GetExamples()
        {
            return new RacaEntity
            {
                Id = 1,
                Raca = "Labrador Retriever",
                IdEspecie = 1,
                Especie = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" },
                Porte = "GRANDE",
                StAtivo = "S"
            };
        }
    }

    public class RacaByIdResponseSample : IExamplesProvider<GetRacaDto>
    {
        public GetRacaDto GetExamples()
        {
            return new RacaResponseSample().GetExamples().ToRacaDto(Pagination.ItemLinks("/api/racas", 1, "raca"));
        }
    }
}
