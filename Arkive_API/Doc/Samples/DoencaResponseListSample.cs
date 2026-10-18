using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class DoencaResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<GetDoencaDto>>>
    {
        public PageResultModel<IEnumerable<GetDoencaDto>> GetExamples()
        {
            var categoria = new CategoriaDoencaEntity { Id = 1, Nome = "Doenças Ortopédicas", StAtivo = "S" };

            var data = new List<DoencaEntity>
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

            var links = Pagination.Links("/api/doencas", 0, 2, 10);
            links.Add(new("/api/doencas", "create_doenca", "POST"));

            return new PageResultModel<IEnumerable<GetDoencaDto>>
            {
                Data = data.Select(x => x.ToDoencaDto(Pagination.ItemLinks("/api/doencas", x.Id, "doenca", ativo: x.StAtivo == "S", naLista: true))).ToList(),
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = links
            };
        }
    }
}
