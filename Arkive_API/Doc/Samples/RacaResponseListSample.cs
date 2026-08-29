using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class RacaResponseListSample : IExamplesProvider<IEnumerable<RacaEntity>>
    {
        public IEnumerable<RacaEntity> GetExamples()
        {
            var canina = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" };

            return new List<RacaEntity>
            {
                new RacaEntity { Id = 1, Raca = "Labrador Retriever", IdEspecie = 1, Especie = canina, Porte = "GRANDE", StAtivo = "S" },
                new RacaEntity { Id = 2, Raca = "Poodle", IdEspecie = 1, Especie = canina, Porte = "PEQUENO", StAtivo = "S" },
            };
        }
    }
}
