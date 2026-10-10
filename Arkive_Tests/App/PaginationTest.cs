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
        public void Normalizar_QuandoTakeZeradoOuNegativo_AplicaTakePadrao()
        {
            // Act
            var (skip, take) = Pagination.Normalizar(10, 0);

            // Assert
            Assert.Equal(10, skip);
            Assert.Equal(Pagination.TakePadrao, take);
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void Links_GeraNextEPrevConformeLimites()
        {
            // Act
            var primeira = Pagination.Links("/x", 0, 10, 25);
            var ultima = Pagination.Links("/x", 20, 10, 25);

            // Assert
            Assert.Equal(new[] { "self", "next" }, primeira.Select(l => l.Rel));
            Assert.Equal("/x?skip=10&take=10", primeira[1].Href);
            Assert.Equal(new[] { "self", "prev" }, ultima.Select(l => l.Rel));
            Assert.Equal("/x?skip=10&take=10", ultima[1].Href);
        }
    }
}
