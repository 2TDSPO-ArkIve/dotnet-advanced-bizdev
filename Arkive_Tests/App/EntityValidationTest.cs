using System.ComponentModel.DataAnnotations;
using Arkive_API.Domain.Entities;

namespace Arkive_Tests.App
{
    /// <summary>
    /// Testes da camada de Domínio: as regras de validação declaradas via DataAnnotations
    /// nas entidades (Range, RegularExpression, StringLength, Required).
    /// </summary>
    public class EntityValidationTest
    {
        private static (bool ok, IList<ValidationResult> erros) Validar(object entidade)
        {
            var contexto = new ValidationContext(entidade);
            var resultados = new List<ValidationResult>();
            var ok = Validator.TryValidateObject(entidade, contexto, resultados, validateAllProperties: true);
            return (ok, resultados);
        }

        [Theory]
        [Trait("Domain", "Validacao")]
        [InlineData(0, true)]
        [InlineData(10, true)]
        [InlineData(-1, false)]
        [InlineData(11, false)]
        public void FeedbackNPS_Nota_DeveAceitarSomenteIntervalo0a10(int nota, bool esperadoValido)
        {
            // Arrange
            var feedback = new FeedbackNPSEntity { Nota = nota, IdResponsavel = 1 };

            // Act
            var (ok, erros) = Validar(feedback);

            // Assert
            Assert.Equal(esperadoValido, ok);
            if (!esperadoValido)
                Assert.Contains(erros, e => e.MemberNames.Contains(nameof(FeedbackNPSEntity.Nota)));
        }

        [Theory]
        [Trait("Domain", "Validacao")]
        [InlineData("PEQUENO", true)]
        [InlineData("medio", true)]   // regex é case-insensitive: (?i)
        [InlineData("GRANDE", true)]
        [InlineData("GIGANTE", false)]
        [InlineData(null, true)]      // Porte é opcional
        public void Raca_Porte_DeveAceitarSomenteValoresPermitidos(string? porte, bool esperadoValido)
        {
            // Arrange
            var raca = new RacaEntity { Raca = "Poodle", IdEspecie = 1, Porte = porte };

            // Act
            var (ok, _) = Validar(raca);

            // Assert
            Assert.Equal(esperadoValido, ok);
        }

        [Fact]
        [Trait("Domain", "Validacao")]
        public void Raca_Nome_DeveSerInvalido_QuandoExcede50Caracteres()
        {
            // Arrange
            var raca = new RacaEntity { Raca = new string('a', 51), IdEspecie = 1 };

            // Act
            var (ok, erros) = Validar(raca);

            // Assert
            Assert.False(ok);
            Assert.Contains(erros, e => e.MemberNames.Contains(nameof(RacaEntity.Raca)));
        }

        [Fact]
        [Trait("Domain", "Validacao")]
        public void Doenca_Nome_DeveSerObrigatorio()
        {
            // Arrange
            var doenca = new DoencaEntity { Nome = null! };

            // Act
            var (ok, erros) = Validar(doenca);

            // Assert
            Assert.False(ok);
            Assert.Contains(erros, e => e.MemberNames.Contains(nameof(DoencaEntity.Nome)));
        }
    }
}
