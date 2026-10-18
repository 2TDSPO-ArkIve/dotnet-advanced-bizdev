using Arkive_API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Arkive_API.Doc.Samples
{
    public class AuthRequestSample : IExamplesProvider<PostAuthDto>
    {
        public PostAuthDto GetExamples()
        {
            return new PostAuthDto
            {
                Login = "admin@arkive.com",
                Senha = "SenhaForte@123"
            };
        }
    }
}
