using Arkive_API.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class DoencaResponseSample : IExamplesProvider<DoencaEntity>
    {
        public DoencaEntity GetExamples()
        {
            return new DoencaEntity
            {
                Id = 1,
                Nome = "Displasia Coxofemoral",
                IdCategoria = 1,
                Categoria = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" },
                Descricao = "Malformação da articulação do quadril, comum em raças de grande porte.",
                CID = "M16",
                Sintomas = "Dificuldade para subir escadas, claudicação, dor ao levantar.",
                StAtivo = "S"
            };
        }
    }
}
