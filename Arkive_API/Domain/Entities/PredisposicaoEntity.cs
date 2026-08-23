using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arkive_API.Domain.Entities
{
    [Table("TB_ARKIVE_PREDISPOSICAO")]
    [Index(nameof(IdEspecie), nameof(IdRaca), nameof(IdDoenca), IsUnique = true, Name = "UX_ARKIVE_PREDISPOSICAO")]
    public class PredisposicaoEntity
    {
        [Key]
        [Column("ID_PREDISPOSICAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O ID da Espécie é obrigatório.")]
        [Column("ID_ESPECIE")]
        public int IdEspecie { get; set; }

        [ForeignKey(nameof(IdEspecie))]
        public EspecieEntity? Especie { get; set; }

        [Column("ID_RACA")]
        public int? IdRaca { get; set; }

        [ForeignKey(nameof(IdRaca))]
        public RacaEntity? Raca { get; set; }

        [Required(ErrorMessage = "O ID da Doença é obrigatório.")]
        [Column("ID_DOENCA")]
        public int IdDoenca { get; set; }

        [ForeignKey(nameof(IdDoenca))]
        public DoencaEntity? Doenca { get; set; }
    }
}