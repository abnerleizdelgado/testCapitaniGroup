using Lottery.Models;
using MongoDB.Driver;
namespace Lottery.Services
{


    public class LotteryService
    {
        private readonly IMongoCollection<Models.Lottery> _lotteryCollection;
        private readonly IMongoCollection<Player> _playerCollection;
        public LotteryService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("test");
            _lotteryCollection = database.GetCollection<Models.Lottery>("Lottery");
            _playerCollection = database.GetCollection<Player>("Players");
        }

        public async Task<List<Models.Lottery>> GetAllLotteriesAsync()
        {
            return await _lotteryCollection.Find(l => true).ToListAsync();
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _playerCollection.Find(p => true).ToListAsync();
        }
        public async Task<(List<Player>, List<Models.Lottery>)> GetResultync()
        {
            var players = await _playerCollection.Find(p => true).ToListAsync();
            var lotteries =  await _lotteryCollection.Find(l => true).ToListAsync();


            foreach (var player in players)
            {
                player.Won = false;

                foreach (var lottery in lotteries)
                {
                    if (lottery.WinningNumber != null && lottery.WinningNumber.HasValue)
                    {
                        if (player.YourLuckyNumbers.Any(x => x.LuckyNumber.Equals(lottery.WinningNumber)))
                        {
                            player.Won = true;
                            break; 
                        }
                    }
                }
            }
            return new(players, lotteries);
        }
    }
}
