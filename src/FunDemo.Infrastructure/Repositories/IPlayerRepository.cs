using FunDemo.Domain.Aggregates.Player;

namespace FunDemo.Infrastructure.Repositories;

public interface IPlayerRepository
{
    void Create(Player player);
    Task<Player> GetByUserIdAsync(UserId userId);
    Task UpdateLocation(Player player);
    Task SaveChanges();
}