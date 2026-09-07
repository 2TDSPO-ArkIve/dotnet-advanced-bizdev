using Arkive_API.Application.Dtos;
using Arkive_API.Application.Mappers;

namespace Arkive_Tests.App
{
    /// <summary>
    /// Testes da camada de Domínio/Application: os mappers que convertem RequestDto em Entity.
    /// Foco na lógica de conversão (normalização de campos, valores default).
    /// </summary>
    public class MapperTest
    {
        [Fact]
        [Trait("Domain", "Mappers")]
        public void DoencaMapper_DeveMapearTodosOsCampos()
        {
            // Arrange
            var dto = new DoencaRequestDto
            {
                Nome = "Cinomose",
                IdCategoria = 3,
                Descricao = "Doença viral",
                CID = "A01",
                Sintomas = "Febre, tosse"
            };

            // Act
            var entity = dto.ToDoencaEntity();

            // Assert
            Assert.Equal("Cinomose", entity.Nome);
            Assert.Equal(3, entity.IdCategoria);
            Assert.Equal("Doença viral", entity.Descricao);
            Assert.Equal("A01", entity.CID);
            Assert.Equal("Febre, tosse", entity.Sintomas);
            Assert.Equal("S", entity.StAtivo); // default da entidade, não vem do DTO
        }

        [Theory]
        [Trait("Domain", "Mappers")]
        [InlineData("pequeno", "PEQUENO")]
        [InlineData("Médio", "MÉDIO")]
        [InlineData(null, null)]
        public void RacaMapper_DeveNormalizarPorteParaMaiusculas(string? porteEntrada, string? porteEsperado)
        {
            // Arrange
            var dto = new RacaRequestDto { Raca = "Poodle", IdEspecie = 1, Porte = porteEntrada };

            // Act
            var entity = dto.ToRacaEntity();

            // Assert
            Assert.Equal("Poodle", entity.Raca);
            Assert.Equal(1, entity.IdEspecie);
            Assert.Equal(porteEsperado, entity.Porte);
        }

        [Fact]
        [Trait("Domain", "Mappers")]
        public void FeedbackNPSMapper_NaoDeveZerarDataFeedback()
        {
            // Arrange
            var antes = DateTime.Now.AddSeconds(-1);
            var dto = new FeedbackNPSRequestDto { Nota = 8, IdResponsavel = 1, Comentario = "Bom" };

            // Act
            var entity = dto.ToFeedbackNPSEntity();

            // Assert
            Assert.Equal(8, entity.Nota);
            Assert.Equal(1, entity.IdResponsavel);
            Assert.Equal("Bom", entity.Comentario);
            // DataFeedback não é mapeada — mantém o default DateTime.Now da entidade
            Assert.NotEqual(default, entity.DataFeedback);
            Assert.True(entity.DataFeedback >= antes);
        }

        [Fact]
        [Trait("Domain", "Mappers")]
        public void CategoriaDoencaMapper_DeveMapearNome()
        {
            var entity = new CategoriaDoencaRequestDto { Nome = "Viral" }.ToCategoriaDoencaEntity();

            Assert.Equal("Viral", entity.Nome);
            Assert.Equal("S", entity.StAtivo);
        }

        [Fact]
        [Trait("Domain", "Mappers")]
        public void EspecieMapper_DeveMapearEspecie()
        {
            var entity = new EspecieRequestDto { Especie = "Canina" }.ToEspecieEntity();

            Assert.Equal("Canina", entity.Especie);
            Assert.Equal("S", entity.StAtivo);
        }
    }
}
