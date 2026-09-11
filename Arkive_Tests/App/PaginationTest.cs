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
        public void Normalizar_LimitaTakeAoMaximo()
        {
            // Act
            var (skip, take) = Pagination.Normalizar(-5, 999);

            // Assert
            Assert.Equal(0, skip);
            Assert.Equal(Pagination.TakeMaximo, take);
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void Normalizar_ForcaTakeMinimoDeUm()
        {
            // Act
            var (skip, take) = Pagination.Normalizar(10, 0);

            // Assert
            Assert.Equal(10, skip);
            Assert.Equal(1, take);
        }
    }
}
