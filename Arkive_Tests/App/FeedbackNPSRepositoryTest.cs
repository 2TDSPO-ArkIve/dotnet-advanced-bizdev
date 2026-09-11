using Arkive_API.Domain.Entities;
using Arkive_API.Domain.Entities.External;
using Arkive_API.Infrastructure.Data;
using Arkive_API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Arkive_Tests.App
{
    public class FeedbackNPSRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly FeedbackNPSRepository _feedbackNPSRepository;

        public FeedbackNPSRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "FeedbackNPSTestDatabase")
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            // Isso é o que vamos testar
            _feedbackNPSRepository = new FeedbackNPSRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterTodosAsync_DeveRetornarTodosOsFeedbacks()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdResponsavel = 1 },
                new FeedbackNPSEntity { Nota = 5, IdAnimal = 1 }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalRegistros);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdResponsavel); },
                item => { Assert.Equal(5, item.Nota); Assert.Equal(1, item.IdAnimal); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorIdAsync_QuandoExiste_DeveRetornarFeedback()
        {
            // Arrange
            var feedback = new FeedbackNPSEntity { Nota = 10, IdResponsavel = 1 };

            _applicationContext.FeedbackNPS.Add(feedback);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorIdAsync(feedback.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(feedback.Id, resultado!.Id);
            Assert.Equal(feedback.Nota, resultado.Nota);
            Assert.Equal(feedback.IdResponsavel, resultado.IdResponsavel);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            // Act
            var resultado = await _feedbackNPSRepository.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorNotaAsync_DeveRetornarFeedbacksComANotaInformada()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdResponsavel = 1 },
                new FeedbackNPSEntity { Nota = 5, IdAnimal = 1 }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorNotaAsync(10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdResponsavel); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorResponsavelAsync_DeveRetornarFeedbacksDoResponsavel()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdResponsavel = 1 },
                new FeedbackNPSEntity { Nota = 5, IdResponsavel = 2 }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorResponsavelAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdResponsavel); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorAnimalAsync_DeveRetornarFeedbacksDoAnimal()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdAnimal = 1 },
                new FeedbackNPSEntity { Nota = 5, IdAnimal = 2 }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorAnimalAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdAnimal); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorClinicaAsync_DeveRetornarFeedbacksDaClinica()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdClinica = 1 },
                new FeedbackNPSEntity { Nota = 5, IdClinica = 2 }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorClinicaAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdClinica); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorVeterinarioAsync_DeveRetornarFeedbacksDoVeterinario()
        {
            // Arrange
            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdVeterinario = 1 },
                new FeedbackNPSEntity { Nota = 5, IdVeterinario = 2 }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorVeterinarioAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdVeterinario); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ObterPorDataAsync_DeveRetornarFeedbacksDaDataInformada()
        {
            // Arrange
            var hoje = DateTime.Today;
            var ontem = hoje.AddDays(-1);

            var feedbacks = new List<FeedbackNPSEntity>
            {
                new FeedbackNPSEntity { Nota = 10, IdResponsavel = 1, DataFeedback = hoje },
                new FeedbackNPSEntity { Nota = 5, IdResponsavel = 1, DataFeedback = ontem }
            };

            _applicationContext.FeedbackNPS.AddRange(feedbacks);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ObterPorDataAsync(hoje);

            // Assert
            Assert.NotNull(resultado);
            Assert.Collection(resultado.Data,
                item => { Assert.Equal(10, item.Nota); Assert.Equal(1, item.IdResponsavel); Assert.Equal(hoje, item.DataFeedback); });
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task AdicionarAsync_DeveAdicionarFeedback()
        {
            // Arrange
            var feedback = new FeedbackNPSEntity { Nota = 9, IdResponsavel = 1, Comentario = "Ótimo atendimento" };

            // Act
            var resultado = await _feedbackNPSRepository.AdicionarAsync(feedback);

            // Assert
            Assert.NotNull(resultado);

            var feedbackNoDb = _applicationContext.FeedbackNPS.FirstOrDefault(x => x.Id == resultado!.Id);

            Assert.NotNull(feedbackNoDb);
            Assert.Equal(9, feedbackNoDb!.Nota);
            Assert.Equal("Ótimo atendimento", feedbackNoDb.Comentario);
            Assert.Equal(1, feedbackNoDb.IdResponsavel);
            Assert.NotEqual(default, feedbackNoDb.DataFeedback);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task DeletarAsync_QuandoExiste_DeveDeletarFeedback()
        {
            // Arrange
            var feedback = new FeedbackNPSEntity { Nota = 9, IdResponsavel = 1 };
            _applicationContext.FeedbackNPS.Add(feedback);
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.DeletarAsync(feedback.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(feedback.Id, resultado!.Id);
            Assert.Equal(feedback.Nota, resultado.Nota);

            var feedbackNoDb = _applicationContext.FeedbackNPS.FirstOrDefault(x => x.Id == feedback.Id);
            Assert.Null(feedbackNoDb);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task DeletarAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            // Act
            var resultado = await _feedbackNPSRepository.DeletarAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ResponsavelExisteAsync_QuandoExiste_DeveRetornarTrue()
        {
            // Arrange
            _applicationContext.Responsavel.Add(new ResponsavelExternal { Id = 1 });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ResponsavelExisteAsync(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ResponsavelExisteAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Act
            var resultado = await _feedbackNPSRepository.ResponsavelExisteAsync(999);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task AnimalExisteAsync_QuandoExiste_DeveRetornarTrue()
        {
            // Arrange
            _applicationContext.Animal.Add(new AnimalExternal { Id = 1 });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.AnimalExisteAsync(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task AnimalExisteAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Act
            var resultado = await _feedbackNPSRepository.AnimalExisteAsync(999);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ClinicaExisteAsync_QuandoExiste_DeveRetornarTrue()
        {
            // Arrange
            _applicationContext.Clinica.Add(new ClinicaExternal { Id = 1 });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ClinicaExisteAsync(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ClinicaExisteAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Act
            var resultado = await _feedbackNPSRepository.ClinicaExisteAsync(999);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ConsultaExisteAsync_QuandoExiste_DeveRetornarTrue()
        {
            // Arrange
            _applicationContext.Consulta.Add(new ConsultaExternal { Id = 1 });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.ConsultaExisteAsync(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task ConsultaExisteAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Act
            var resultado = await _feedbackNPSRepository.ConsultaExisteAsync(999);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task VeterinarioExisteAsync_QuandoExiste_DeveRetornarTrue()
        {
            // Arrange
            _applicationContext.Veterinario.Add(new VeterinarioExternal { Id = 1 });
            _applicationContext.SaveChanges();

            // Act
            var resultado = await _feedbackNPSRepository.VeterinarioExisteAsync(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        [Trait("Repository", "FeedbackNPS")]
        public async Task VeterinarioExisteAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            // Act
            var resultado = await _feedbackNPSRepository.VeterinarioExisteAsync(999);

            // Assert
            Assert.False(resultado);
        }
    }
}