using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arkive_API.Domain.Entities
{
    [Table("TB_ARKIVE_FEEDBACK_NPS")]
    public class FeedbackNPSEntity
    {
        [Key]
        [Column("ID_FEEDBACK_NPS")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("ID_RESPONSAVEL")]
        public int? IdResponsavel { get; set; }

        [Column("ID_ANIMAL")]
        public int? IdAnimal { get; set; }

        [Column("ID_CLINICA")]
        public int? IdClinica { get; set; }

        [Column("ID_CONSULTA")]
        public int? IdConsulta { get; set; }

        [Column("ID_VETERINARIO")]
        public int? IdVeterinario { get; set; }

        [Required(ErrorMessage = "A Nota é obrigatória.")]
        [Column("NR_NOTA")]
        [Range(0, 10, ErrorMessage = "A Nota deve estar entre 0 e 10")]
        public int Nota { get; set; }

        [Column("DS_COMENTARIO", TypeName = "CLOB")]
        public string? Comentario { get; set; }

        [Required(ErrorMessage = "A Data do Feedback é obrigatória")]
        [Column("DT_FEEDBACK")]
        public DateTime DataFeedback { get; set; } = DateTime.Now;
    }
}