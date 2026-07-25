using FunDemo.Domain.Aggregates.Player;
using System.Security.Claims;

namespace FunDemo.ApiService.Extensions;

public static class UserExtensions
{
    public static UserId GetUserId(this ClaimsPrincipal user)
    {
        return new UserId(user.Claims.First(x => x.Type == "sub").Value);
    }
}
