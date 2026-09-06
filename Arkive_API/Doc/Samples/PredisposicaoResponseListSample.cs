using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class PredisposicaoResponseListSample : IExamplesProvider<IEnumerable<PredisposicaoEntity>>
    {
        public IEnumerable<PredisposicaoEntity> GetExamples()
        {
            var especieCanina = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" };
            var categoriaOrtopedica = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" };
            var labrador = new RacaEntity { Id = 1, Raca = "Labrador Retriever", IdEspecie = 1, Especie = especieCanina, Porte = "GRANDE", StAtivo = "S" };
            var displasia = new DoencaEntity { Id = 1, Nome = "Displasia Coxofemoral", IdCategoria = 1, Categoria = categoriaOrtopedica, Descricao = "Malformação da articulação do quadril, comum em raças de grande porte.", CID = "M16", Sintomas = "Dificuldade para subir escadas, claudicação, dor ao levantar.", StAtivo = "S" };

            return new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity
                {
                    Id = 1,
                    IdEspecie = 1,
                    Especie = especieCanina,
                    IdRaca = 1,
                    Raca = labrador,
                    IdDoenca = 1,
                    Doenca = displasia
                },
                new PredisposicaoEntity
                {
                    Id = 2,
                    IdEspecie = 1,
                    Especie = especieCanina,
                    IdRaca = null,
                    Raca = null,
                    IdDoenca = 1,
                    Doenca = displasia
                },
            };
        }
    }
}
