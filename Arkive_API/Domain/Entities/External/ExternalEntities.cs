using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arkive_API.Domain.Entities.External
{
    [Table("TB_ARKIVE_RESPONSAVEL")]
    public class ResponsavelExternal
    {
        [Key]
        [Column("ID_RESPONSAVEL")]
        public int Id { get; set; }
    }

    [Table("TB_ARKIVE_ANIMAL")]
    public class AnimalExternal
    {
        [Key]
        [Column("ID_ANIMAL")]
        public int Id { get; set; }
    }

    [Table("TB_ARKIVE_CLINICA")]
    public class ClinicaExternal
    {
        [Key]
        [Column("ID_CLINICA")]
        public int Id { get; set; }
    }

    [Table("TB_ARKIVE_CONSULTA")]
    public class ConsultaExternal
    {
        [Key]
        [Column("ID_CONSULTA")]
        public int Id { get; set; }
    }

    [Table("TB_ARKIVE_VETERINARIO")]
    public class VeterinarioExternal
    {
        [Key]
        [Column("ID_VETERINARIO")]
        public int Id { get; set; }
    }
}