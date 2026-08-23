using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Arkive_API.Domain.Entities
{
    [Table("TB_ARKIVE_ESPECIE")]
    [Index(nameof(Especie), IsUnique = true, Name = "UQ_ARKIVE_ESPECIE_NOME")]
    public class EspecieEntity
    {
        [Key]
        [Column("ID_ESPECIE")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da Espécie é obrigatório.")]
        [Column("NM_ESPECIE")]
        [StringLength(50, ErrorMessage = "O nome da Espécie deve ter no máximo 50 caracteres.")]
        public string Especie { get; set; }

        [Column("ST_ATIVO")]
        [Required]
        [MaxLength(1)]
        public string StAtivo { get; set; } = "S";

        [JsonIgnore]
        public ICollection<RacaEntity>? Racas { get; set; }

        [JsonIgnore]
        public ICollection<PredisposicaoEntity>? Predisposicoes { get; set; }
    }
}