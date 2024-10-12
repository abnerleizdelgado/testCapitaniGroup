using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lottery.Models
{
    public class Lottery
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("drawDate")]
        public DateTime DrawDate { get; set; }

        [BsonElement("drawNumber")]
        public int DrawNumber { get; set; }

        [BsonElement("winningNumber")]
        public int? WinningNumber { get; set; } 
    }
}
