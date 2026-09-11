using Arkive_API.Domain.Entities;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class PredisposicaoRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly PredisposicaoRepository _predisposicaoRepository;

        public PredisposicaoRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _predisposicaoRepository = new PredisposicaoRepository(_applicationContext);

            // Semeia as entidades relacionadas (FKs obrigatórias em PredisposicaoEntity).
            // Sem isso, o Include(Especie)/Include(Doenca) do repositório não encontra
            // a entidade pai e a Predisposicao some do resultado (relacionamento obrigatório).
            SemearDadosRelacionados();
        }

        private void SemearDadosRelacionados()
        {
            _applicationContext.Especie.AddRange(
                new EspecieEntity { Id = 1, Especie = "Canina" },
                new EspecieEntity { Id = 2, Especie = "Felina" }
            );

            _applicationContext.Raca.AddRange(
                new RacaEntity { Id = 1, IdEspecie = 1, Raca = "Labrador" },
                new RacaEntity { Id = 2, IdEspecie = 1, Raca = "Poodle" }
            );

            _applicationContext.Doenca.AddRange(
                new DoencaEntity { Id = 1, Nome = "Displasia Coxofemoral" },
                new DoencaEntity { Id = 2, Nome = "Otite" }
            );

            _applicationContext.SaveChanges();
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task ObterTodosAsync_DeveRetornarTodasAsPredisposicoes()
        {
            // Arrange
            var predisposicoes = new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { IdEspecie = 1, IdDoenca = 1 },
                new PredisposicaoEntity { IdEspecie = 1, IdRaca = 1, IdDoenca = 2 }
            };

            _applicationContext.Predisposicao.AddRange(predisposicoes);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _predisposicaoRepository.ObterTodosAsync();

            // Assert — repositório ordena por Id ascendente (Skip/Take determinístico)
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalRegistros);
            Assert.Collection(resultado.Data,
                item =>
                {
                    Assert.Equal(1, item.IdEspecie);
                    Assert.Null(item.IdRaca);
                    Assert.Equal(1, item.IdDoenca);
                    Assert.Null(item.Raca);
                    Assert.NotNull(item.Especie);
                    Assert.Equal("Canina", item.Especie!.Especie);
                    Assert.NotNull(item.Doenca);
                    Assert.Equal("Displasia Coxofemoral", item.Doenca!.Nome);
                },
                item =>
                {
                    Assert.Equal(1, item.IdEspecie);
                    Assert.Equal(1, item.IdRaca);
                    Assert.Equal(2, item.IdDoenca);
                    Assert.NotNull(item.Especie);
                    Assert.Equal("Canina", item.Especie!.Especie);
                    Assert.NotNull(item.Raca);
                    Assert.Equal("Labrador", item.Raca!.Raca);
                    Assert.NotNull(item.Doenca);
                    Assert.Equal("Otite", item.Doenca!.Nome);
                });
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task ObterPorIdAsync_DeveRetornarPredisposicao_QuandoExiste()
        {
            // Arrange
            var predisposicao = new PredisposicaoEntity { IdEspecie = 1, IdDoenca = 1 };

            _applicationContext.Predisposicao.Add(predisposicao);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _predisposicaoRepository.ObterPorIdAsync(predisposicao.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(predisposicao.Id, resultado!.Id);
            Assert.Equal(1, resultado.IdEspecie);
            Assert.Null(resultado.IdRaca);
            Assert.Equal(1, resultado.IdDoenca);
            Assert.Null(resultado.Raca);

            // Garante que os relacionamentos obrigatórios foram carregados (Include)
            Assert.NotNull(resultado.Especie);
            Assert.Equal(1, resultado.Especie!.Id);
            Assert.Equal("Canina", resultado.Especie.Especie);
            Assert.NotNull(resultado.Doenca);
            Assert.Equal(1, resultado.Doenca!.Id);
            Assert.Equal("Displasia Coxofemoral", resultado.Doenca.Nome);
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Act
            var resultado = await _predisposicaoRepository.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task ObterPorEspecieAsync_DeveRetornarPredisposicoesDaEspecie()
        {
            // Arrange
            var predisposicoes = new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { IdEspecie = 1, IdDoenca = 1 },
                new PredisposicaoEntity { IdEspecie = 2, IdDoenca = 2 }
            };

            _applicationContext.Predisposicao.AddRange(predisposicoes);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _predisposicaoRepository.ObterPorEspecieAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data, item =>
            {
                Assert.Equal(1, item.IdEspecie);
                Assert.Null(item.IdRaca);
                Assert.Equal(1, item.IdDoenca);
                Assert.NotNull(item.Especie);
                Assert.Equal("Canina", item.Especie!.Especie);
                Assert.NotNull(item.Doenca);
                Assert.Equal("Displasia Coxofemoral", item.Doenca!.Nome);
            });
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task ObterPorRacaAsync_DeveRetornarPredisposicoesDaRaca()
        {
            // Arrange
            var predisposicoes = new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { IdEspecie = 1, IdRaca = 1, IdDoenca = 1 },
                new PredisposicaoEntity { IdEspecie = 1, IdRaca = 2, IdDoenca = 2 }
            };

            _applicationContext.Predisposicao.AddRange(predisposicoes);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _predisposicaoRepository.ObterPorRacaAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data, item =>
            {
                Assert.Equal(1, item.IdEspecie);
                Assert.Equal(1, item.IdRaca);
                Assert.Equal(1, item.IdDoenca);
                Assert.NotNull(item.Especie);
                Assert.Equal("Canina", item.Especie!.Especie);
                Assert.NotNull(item.Raca);
                Assert.Equal("Labrador", item.Raca!.Raca);
                Assert.NotNull(item.Doenca);
                Assert.Equal("Displasia Coxofemoral", item.Doenca!.Nome);
            });
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task ObterPorDoencaAsync_DeveRetornarPredisposicoesDaDoenca()
        {
            // Arrange
            var predisposicoes = new List<PredisposicaoEntity>
            {
                new PredisposicaoEntity { IdEspecie = 1, IdDoenca = 1 },
                new PredisposicaoEntity { IdEspecie = 1, IdDoenca = 2 }
            };

            _applicationContext.Predisposicao.AddRange(predisposicoes);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _predisposicaoRepository.ObterPorDoencaAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data, item =>
            {
                Assert.Equal(1, item.IdEspecie);
                Assert.Null(item.IdRaca);
                Assert.Equal(1, item.IdDoenca);
                Assert.NotNull(item.Especie);
                Assert.Equal("Canina", item.Especie!.Especie);
                Assert.NotNull(item.Doenca);
                Assert.Equal("Displasia Coxofemoral", item.Doenca!.Nome);
            });
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task AdicionarAsync_DeveAdicionarPredisposicao()
        {
            // Arrange
            var predisposicao = new PredisposicaoEntity { IdEspecie = 1, IdRaca = 1, IdDoenca = 1 };

            // Act
            var resultado = await _predisposicaoRepository.AdicionarAsync(predisposicao);

            // Assert
            Assert.NotNull(resultado);

            // Garante que os relacionamentos foram carregados na entidade retornada
            Assert.NotNull(resultado!.Especie);
            Assert.Equal("Canina", resultado.Especie!.Especie);
            Assert.NotNull(resultado.Raca);
            Assert.Equal("Labrador", resultado.Raca!.Raca);
            Assert.NotNull(resultado.Doenca);
            Assert.Equal("Displasia Coxofemoral", resultado.Doenca!.Nome);

            var predisposicaoNoDb = _applicationContext.Predisposicao.FirstOrDefault(x => x.Id == resultado!.Id);

            Assert.NotNull(predisposicaoNoDb);
            Assert.Equal(1, predisposicaoNoDb!.IdEspecie);
            Assert.Equal(1, predisposicaoNoDb.IdRaca);
            Assert.Equal(1, predisposicaoNoDb.IdDoenca);
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task DeletarAsync_DeveDeletarPredisposicao_QuandoExiste()
        {
            // Arrange
            var predisposicao = new PredisposicaoEntity { IdEspecie = 1, IdDoenca = 1 };
            _applicationContext.Predisposicao.Add(predisposicao);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _predisposicaoRepository.DeletarAsync(predisposicao.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(predisposicao.Id, resultado!.Id);
            Assert.Equal(1, resultado.IdEspecie);
            Assert.Equal(1, resultado.IdDoenca);

            var predisposicaoNoDb = _applicationContext.Predisposicao.FirstOrDefault(x => x.Id == predisposicao.Id);
            Assert.Null(predisposicaoNoDb);
        }

        [Fact]
        [Trait("Repository", "Predisposicoes")]
        public async Task DeletarAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Act
            var resultado = await _predisposicaoRepository.DeletarAsync(999);

            // Assert
            Assert.Null(resultado);
        }
    }
}