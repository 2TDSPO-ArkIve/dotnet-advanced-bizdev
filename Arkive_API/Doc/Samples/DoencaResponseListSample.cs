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
                    Descricao = "Malformação da articulação do quadril, comum em raças de grande porte.",
                    CID = "M16",
                    Sintomas = "Dificuldade para subir escadas, claudicação, dor ao levantar.",
                    StAtivo = "S"
                },
                new DoencaEntity
                {
                    Id = 2,
                    Nome = "Luxação de Patela",
                    IdCategoria = 1,
                    Categoria = categoria,
                    Descricao = "Deslocamento da patela para fora da tróclea femoral, frequente em raças de pequeno porte.",
                    CID = "M22",
                    Sintomas = "Claudicação intermitente, pata suspensa durante a corrida, dificuldade de estender o joelho.",
                    StAtivo = "S"
                },
            };
        }
    }
}
