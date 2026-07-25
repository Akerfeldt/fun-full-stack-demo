using FunDemo.ApiService.Extensions;
using FunDemo.Domain.Aggregates.Player;
using FunDemo.Infrastructure.Repositories;

namespace FunDemo.ApiService.Controllers.Me;

public abstract class PlayerController(IPlayerRepository playerRepository) : UserController
{
    protected IPlayerRepository PlayerRepository { get; } = playerRepository;

    protected Task<Player> GetPlayer()
    {
        return PlayerRepository.GetByUserIdAsync(User.GetUserId());
    }
}
