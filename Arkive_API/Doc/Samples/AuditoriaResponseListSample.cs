using Arkive_API.Application;
using Arkive_API.Application.Dtos;
using Arkive_API.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class AuditoriaResponseListSample : IExamplesProvider<PageResultModel<IEnumerable<GetAuditoriaDto>>>
    {
        public PageResultModel<IEnumerable<GetAuditoriaDto>> GetExamples()
        {
            var data = new List<GetAuditoriaDto>
            {
                new() { Id = "665f1c2e8b3a4d0012a1b2c3", Usuario = "admin@arkive.com", Metodo = "POST", Rota = "/api/racas", StatusCode = 201, Data = new DateTime(2026, 6, 1, 12, 0, 0, DateTimeKind.Utc),
                    Requisicao = "{\"Raca\":\"Labrador\",\"IdEspecie\":1,\"Porte\":\"Grande\"}",
                    Resposta = "{\"Id\":7,\"Raca\":\"Labrador\",\"IdEspecie\":1,\"Porte\":\"Grande\",\"StAtivo\":\"S\"}" },
                new() { Id = "665f1c2e8b3a4d0012a1b2c4", Usuario = "admin@arkive.com", Metodo = "POST", Rota = "/api/auth", StatusCode = 200, Data = new DateTime(2026, 6, 1, 11, 55, 0, DateTimeKind.Utc) },
            };

            return new PageResultModel<IEnumerable<GetAuditoriaDto>>
            {
                Data = data,
                Deslocamento = 0,
                RegistroRetornado = 2,
                TotalRegistros = 10,
                Links = Pagination.Links("/api/auditoria", 0, 2, 10)
            };
        }
    }
}
