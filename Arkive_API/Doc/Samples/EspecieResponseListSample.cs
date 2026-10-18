using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;
using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class EspecieResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<GetEspecieDto>>>
    {
        public PageResultModel<IEnumerable<GetEspecieDto>> GetExamples()
        {
            var data = new List<EspecieEntity>
            {
                new EspecieEntity { Id = 1, Especie = "Canina", StAtivo = "S" },
                new EspecieEntity { Id = 2, Especie = "Felina", StAtivo = "S" },
            };

            var links = Pagination.Links("/api/especies", 0, 2, 10);
            links.Add(new("/api/especies", "create_especie", "POST"));

            return new PageResultModel<IEnumerable<GetEspecieDto>>
            {
                Data = data.Select(x => x.ToEspecieDto(Pagination.ItemLinks("/api/especies", x.Id, "especie", ativo: x.StAtivo == "S", naLista: true))).ToList(),
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = links
            };
        }
    }
}
