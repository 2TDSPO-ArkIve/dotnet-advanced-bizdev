using Arkive_Application;
using Arkive_Application.Dtos;
using Arkive_Application.Mappers;
using Arkive_Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class EspecieResponseSample : IExamplesProvider<EspecieEntity>
    {
        public EspecieEntity GetExamples()
        {
            return new EspecieEntity
            {
                Id = 1,
                Especie = "Canina",
                StAtivo = "S"
            };
        }
    }

    public class EspecieByIdResponseSample : IExamplesProvider<GetEspecieDto>
    {
        public GetEspecieDto GetExamples()
        {
            return new EspecieResponseSample().GetExamples().ToEspecieDto(Pagination.ItemLinks("/api/especies", 1, "especie"));
        }
    }
}
