using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunDemo.ApiService.Controllers.Me
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
    }
}
