namespace Arkive_API.Application
{
    /// <summary>
    /// Perfis de usuário (TP_USUARIO de TB_ARKIVE_USUARIO), usados em [Authorize(Roles = ...)].
    /// </summary>
    public static class Roles
    {
        public const string SysAdmin = "SYSADMIN";
        public const string Veterinario = "VETERINARIO";

        /// <summary>Catálogo clínico que o veterinário também mantém (Espécie, Raça, Doença).</summary>
        public const string SysAdminOuVeterinario = SysAdmin + "," + Veterinario;
    }
}
