using FunDemo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunDemo.ApiService.Controllers.Me
{
    public class LocationController(IPlayerRepository playerRepository) : PlayerController(playerRepository)
    {
        [HttpGet("api/me/location")]
        public async Task<IActionResult> GetAsync()
        {
            var player = await GetPlayer();

            if (player is null)
            {
                return NotFound();
            }

            return Ok(player.Location);
        }

        [HttpPost("api/me/location/go-up")]
        public async Task<IActionResult> GoUpAsync()
        {
            var player = await GetPlayer();

            if (player is null)
            {
                return NotFound();
            }

            player.GoUp();
            await UpdateLocation(player);

            return NoContent();
        }

        [HttpPost("api/me/location/go-down")]
        public async Task<IActionResult> GoDownAsync()
        {
            var player = await GetPlayer();

            if (player is null)
            {
                return NotFound();
            }

            player.GoDown();
            await UpdateLocation(player);

            return NoContent();
        }

        [HttpPost("api/me/location/go-left")]
        public async Task<IActionResult> GoLeftAsync()
        {
            var player = await GetPlayer();

            if (player is null)
            {
                return NotFound();
            }

            player.GoLeft();
            await UpdateLocation(player);

            return NoContent();
        }

        [HttpPost("api/me/location/go-right")]
        public async Task<IActionResult> GoRightAsync()
        {
            var player = await GetPlayer();

            if (player is null)
            {
                return NotFound();
            }

            player.GoRight();
            await UpdateLocation(player);

            return NoContent();
        }

        private async Task UpdateLocation(Domain.Aggregates.Player.Player player)
        {
            await PlayerRepository.UpdateLocation(player);
            await PlayerRepository.SaveChanges();
        }
    }
}
