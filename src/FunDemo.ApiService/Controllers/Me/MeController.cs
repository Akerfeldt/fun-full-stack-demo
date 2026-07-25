using FunDemo.ApiService.Extensions;
using FunDemo.Domain.Aggregates.Player;
using FunDemo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunDemo.ApiService.Controllers.Me
{
    public class MeController(IPlayerRepository playerRepository) : PlayerController(playerRepository)
    {
        [HttpGet("api/me/identity")]
        public IActionResult Get()
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            if (user is null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                UserId = user,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpGet("api/me/character")]
        public async Task<IActionResult> GetCharacterAsync()
        {
            var player = await GetPlayer();

            if (player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost("api/me/character")]
        public async Task<IActionResult> GetCharacterAsync(CharacterDto character)
        {
            PlayerRepository.Create(new Player(
                User.GetUserId(), 
                new PlayerName(character.Name),
                (PlayerClass)character.Class,
                (Race)character.Race));

            await PlayerRepository.SaveChanges();

            return NoContent();
        }
    }

    public class CharacterDto
    {
        public required string Name { get; set; }
        public RaceDto Race { get; set; }
        public ClassDto Class { get; set; }
    }

    public enum ClassDto : byte
    {
        Warrior = 1,
        Mage = 2,
        Priest = 3,
    }

    public enum RaceDto : byte
    {
        Human = 1,
    }
}
