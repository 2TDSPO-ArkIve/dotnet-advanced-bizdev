using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Arkive_API.Domain.Entities
{
    [Table("TB_ARKIVE_DOENCA")]
    [Index(nameof(Nome), IsUnique = true, Name = "UQ_ARKIVE_DOENCA_NOME")]
    public class DoencaEntity
    {
        [Key]
        [Column("ID_DOENCA")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("NM_DOENCA")]
        [Required(ErrorMessage = "O nome da Doença é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome da Doença deve ter entre 1 e 100 caracteres.")]
        public string Nome { get; set; }

        [Column("ID_CATEGORIA")]
        public int? IdCategoria { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        public CategoriaDoencaEntity? Categoria { get; set; }

        [Column("DS_DOENCA", TypeName = "CLOB")]
        public string? Descricao { get; set; }

        [Column("CD_CID_VET")]
        [StringLength(20, ErrorMessage = "O CID deve ter no máximo 20 caracteres.")]
        public string? CID { get; set; }

        [Column("DS_SINTOMAS", TypeName = "CLOB")]
        public string? Sintomas { get; set; }

        [Column("ST_ATIVO")]
        [Required]
        [MaxLength(1)]
        public string StAtivo { get; set; } = "S";

        [JsonIgnore]
        public ICollection<PredisposicaoEntity>? Predisposicoes { get; set; }
    }
}