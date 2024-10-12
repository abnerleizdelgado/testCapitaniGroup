using MongoDB.Bson.Serialization.Attributes;

namespace Lottery.Models
{
    public class YourLuckyNumber
    {
        [BsonElement("luckyNumber")]
        public int LuckyNumber { get; set; }

        [BsonElement("received")]
        public DateTime Received { get; set; }
    }
}
