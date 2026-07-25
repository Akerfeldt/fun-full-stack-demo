using FunDemo.Domain.Aggregates.Player;
using Microsoft.EntityFrameworkCore;

namespace FunDemo.Infrastructure.Repositories;

public class PlayerRepository(FunContext dbContext) : IPlayerRepository
{
    public void Create(Player player)
    {
        var dbPlayer = new DbPlayer
        {
            Class = (byte)player.Class,
            LocationX = player.Location.X,
            LocationY = player.Location.Y,
            Name = player.Name.Value,
            Race = (byte)player.Race,
            UserId = player.UserId.Value,
        };

        dbContext.Players.Add(dbPlayer);
    }

    public async Task<Player> GetByUserIdAsync(UserId userId)
    {
        var dbPlayer = await dbContext.Players.FirstOrDefaultAsync(p => p.UserId == userId.Value);
        if (dbPlayer is null)
            return null!;

        return new Player(
            new UserId(dbPlayer.UserId),
            new PlayerName(dbPlayer.Name),
            (PlayerClass)dbPlayer.Class,
            (Race)dbPlayer.Race,
            new Location(dbPlayer.LocationX, dbPlayer.LocationY));
    }

    public async Task UpdateLocation(Player player)
    {
        var dbPlayer = await dbContext.Players.FirstAsync(p => p.UserId == player.UserId.Value);

        dbPlayer.LocationX = player.Location.X;
        dbPlayer.LocationY = player.Location.Y;
    }

    public Task SaveChanges()
    {
        return dbContext.SaveChangesAsync();
    }
}
