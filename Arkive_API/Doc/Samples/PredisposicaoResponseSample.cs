using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class PredisposicaoResponseSample : IExamplesProvider<PredisposicaoEntity>
    {
        public PredisposicaoEntity GetExamples()
        {
            var especie = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" };
            var categoria = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" };

            return new PredisposicaoEntity
            {
                Id = 1,
                IdEspecie = 1,
                Especie = especie,
                IdRaca = 1,
                Raca = new RacaEntity { Id = 1, Raca = "Labrador Retriever", IdEspecie = 1, Especie = especie, Porte = "GRANDE", StAtivo = "S" },
                IdDoenca = 1,
                Doenca = new DoencaEntity { Id = 1, Nome = "Displasia Coxofemoral", IdCategoria = 1, Categoria = categoria, CID = "M16", StAtivo = "S" }
            };
        }
    }
}
