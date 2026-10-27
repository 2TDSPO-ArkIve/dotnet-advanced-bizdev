namespace Arkive_API.Application.Dtos
{
    public class GetAuditoriaDto
    {
        public string Id { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Metodo { get; set; } = string.Empty;
        public string Rota { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Data { get; set; }
        public string? Requisicao { get; set; }
        public string? Resposta { get; set; }
    }
}
