using Microsoft.AspNetCore.Mvc;
using Lottery.Services;
using Lottery.Models;

namespace Lottery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotteryController : ControllerBase
    {
        private readonly LotteryService _lotteryService;

        public LotteryController(LotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }

        [HttpGet("drawLotteries")]
        public async Task<ActionResult<List<Models.Lottery>>> GetAlldrawLotteries()
        {
            var lotteries = await _lotteryService.GetAllLotteriesAsync();
            return Ok(lotteries);
        }

        [HttpGet("players")]
        public async Task<ActionResult<List<Player>>> GetAllPlayers()
        {
            var players = await _lotteryService.GetAllPlayersAsync();
            return Ok(players);
        }
        [HttpGet("result")]
        public async Task<ActionResult<object>> GetResult()
        {
            var result = await _lotteryService.GetResultync();

            return Ok(new
            {
                Lotteries = result.Item1,
                Players = result.Item2
            }) ;
        }

    }
}
