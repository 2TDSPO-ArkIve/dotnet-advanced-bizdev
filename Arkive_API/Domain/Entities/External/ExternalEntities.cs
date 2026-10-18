using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    // Usuários são cadastrados pela API Java; aqui só lemos para autenticar (senha em BCrypt).
    [Table("TB_ARKIVE_USUARIO")]
    public class UsuarioExternal
    {
        [Key]
        [Column("ID_USUARIO")]
        public int Id { get; set; }

        [Column("NM_USUARIO")]
        public string Nome { get; set; } = string.Empty;

        [Column("DS_LOGIN")]
        public string Login { get; set; } = string.Empty;

        [JsonIgnore]
        [Column("DS_SENHA_HASH")]
        public string SenhaHash { get; set; } = string.Empty;

        [Column("TP_USUARIO")]
        public string TipoUsuario { get; set; } = string.Empty;

        [Column("ST_ATIVO")]
        public string StAtivo { get; set; } = "S";
    }
}