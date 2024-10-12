using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lottery.Models
{
    public class Player
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int Vibes { get; set; }
        public List<YourLuckyNumber> YourLuckyNumbers { get; set; }
        public bool Won { get; set; }

    }
}
