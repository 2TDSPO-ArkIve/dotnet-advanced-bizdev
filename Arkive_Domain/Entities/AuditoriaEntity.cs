using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arkive_Domain.Entities
{
    [Table("auditoria")]
    public class AuditoriaEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Metodo { get; set; } = string.Empty;
        public string Rota { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Data { get; set; }
        public string? Requisicao { get; set; }
        public string? Resposta { get; set; }
    }
}
