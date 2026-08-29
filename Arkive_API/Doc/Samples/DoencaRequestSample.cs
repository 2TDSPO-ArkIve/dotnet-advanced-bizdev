using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class DoencaRequestSample : IExamplesProvider<DoencaRequestDto>
    {
        public DoencaRequestDto GetExamples()
        {
            return new DoencaRequestDto
            {
                Nome = "Displasia Coxofemoral",
                IdCategoria = 1,
                Descricao = "Malformação da articulação do quadril, comum em raças de grande porte.",
                CID = "M16",
                Sintomas = "Dificuldade para subir escadas, claudicação, dor ao levantar."
            };
        }
    }
}
