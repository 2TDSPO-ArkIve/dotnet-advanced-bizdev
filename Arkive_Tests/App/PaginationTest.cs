using Arkive_API.Application;

namespace Arkive_Tests.App
{
    public class PaginationTest
    {
        [Fact]
        [Trait("Helper", "Pagination")]
        public void Normalizar_MantemValoresValidos()
        {
            // Act
            var (skip, take) = Pagination.Normalizar(0, 50);

            // Assert
            Assert.Equal(0, skip);
            Assert.Equal(50, take);
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void Normalizar_NaoLimitaTakeAlto_ESkipNegativoViraZero()
        {
            // Act
            var (skip, take) = Pagination.Normalizar(-5, 999);

            // Assert
            Assert.Equal(0, skip);
            Assert.Equal(999, take);
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void Normalizar_AplicaTakePadrao_QuandoTakeZeradoOuNegativo()
        {
            // Act
            var (skip, take) = Pagination.Normalizar(10, 0);

            // Assert
            Assert.Equal(10, skip);
            Assert.Equal(Pagination.TakePadrao, take);
        }
    }
}
