using Arkive_API.Application;
using Arkive_API.Domain.Models;

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

        [Fact]
        [Trait("Helper", "Pagination")]
        public void Links_ComOrdenacao_RepassaParametrosEmTodosOsLinks()
        {
            // Act
            var links = Pagination.Links("/x", 10, 10, 30, "nome", "desc");

            // Assert
            Assert.All(links, l => Assert.EndsWith("&ordenarPor=nome&direcao=desc", l.Href));
            Assert.Equal(new[] { "self", "next", "prev" }, links.Select(l => l.Rel));
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void ItemLinks_SemEdicao_OmitePut()
        {
            // Act
            var editavel = Pagination.ItemLinks("/api/doencas", 3, "doenca");
            var semPut = Pagination.ItemLinks("/api/predisposicoes", 3, "predisposicao", editavel: false);

            // Assert
            Assert.Equal(new[] { "self", "put_doenca", "delete_doenca" }, editavel.Select(l => l.Rel));
            Assert.Equal(new[] { "GET", "PUT", "DELETE" }, editavel.Select(l => l.Method));
            Assert.All(editavel, l => Assert.Equal("/api/doencas/3", l.Href));
            Assert.Equal(new[] { "self", "delete_predisposicao" }, semPut.Select(l => l.Rel));
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void ItemLinks_Inativo_OfereceSomenteReativar()
        {
            // Act
            var links = Pagination.ItemLinks("/api/doencas", 3, "doenca", ativo: false);

            // Assert
            Assert.Equal(new[] { "self", "reativar_doenca" }, links.Select(l => l.Rel));
            Assert.Equal("/api/doencas/reativar/3", links[1].Href);
            Assert.Equal("PUT", links[1].Method);
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void ItemLinks_NaLista_UsaGetNoLugarDeSelf()
        {
            // Act
            var links = Pagination.ItemLinks("/api/doencas", 3, "doenca", naLista: true);

            // Assert
            Assert.Equal(new[] { "get_doenca", "put_doenca", "delete_doenca" }, links.Select(l => l.Rel));
        }

        [Fact]
        [Trait("Helper", "Pagination")]
        public void MapearItens_ConverteDataEMantemPaginacao()
        {
            // Arrange
            var pagina = new PageResultModel<IEnumerable<int>>
            {
                Data = new[] { 1, 2 },
                Deslocamento = 2,
                RegistroRetornado = 2,
                TotalRegistros = 5,
                Links = Pagination.Links("/x", 2, 2, 5)
            };

            // Act
            var resultado = Pagination.MapearItens(pagina, x => $"item{x}");

            // Assert
            Assert.Equal(new[] { "item1", "item2" }, resultado.Data);
            Assert.Equal(2, resultado.Deslocamento);
            Assert.Equal(2, resultado.RegistroRetornado);
            Assert.Equal(5, resultado.TotalRegistros);
            Assert.Same(pagina.Links, resultado.Links);
        }
    }
}
