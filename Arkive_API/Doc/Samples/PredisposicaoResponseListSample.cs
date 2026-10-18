using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class PredisposicaoResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<GetPredisposicaoDto>>>
    {
        public PageResultModel<IEnumerable<GetPredisposicaoDto>> GetExamples()
        {
            var especieCanina = new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" };
            var categoriaOrtopedica = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" };
            var labrador = new RacaEntity { Id = 1, Raca = "Labrador Retriever", IdEspecie = 1, Especie = especieCanina, Porte = "GRANDE", StAtivo = "S" };
            var displasia = new DoencaEntity { Id = 1, Nome = "Displasia Coxofemoral", IdCategoria = 1, Categoria = categoriaOrtopedica, Descricao = "Malformação da articulação do quadril, comum em raças de grande porte.", CID = "M16", Sintomas = "Dificuldade para subir escadas, claudicação, dor ao levantar.", StAtivo = "S" };

            var data = new List<PredisposicaoEntity>
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

            var links = Pagination.Links("/api/predisposicoes", 0, 2, 10);
            links.Add(new("/api/predisposicoes", "create_predisposicao", "POST"));

            return new PageResultModel<IEnumerable<GetPredisposicaoDto>>
            {
                Data = data.Select(x => x.ToPredisposicaoDto(Pagination.ItemLinks("/api/predisposicoes", x.Id, "predisposicao", editavel: false, naLista: true))).ToList(),
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = links
            };
        }
    }
}
