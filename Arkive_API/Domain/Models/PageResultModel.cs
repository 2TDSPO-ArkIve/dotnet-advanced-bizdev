namespace Arkive_API.Domain.Models
{
    public class PageResultModel<T>
    {
        public required T Data { get; set; }
        public int Deslocamento { get; set; }
        public int RegistroRetornado { get; set; }
        public int TotalRegistros { get; set; }
    }
}
