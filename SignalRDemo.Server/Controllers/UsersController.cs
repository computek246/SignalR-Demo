using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Server.Helpers;
using SignalRDemo.Server.Hub;
using SignalRDemo.Server.Models;
using SignalRDemo.Server.Services;

namespace SignalRDemo.Server.Controllers
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {



        private readonly IUserService _userService;
        private readonly IHubContext<Notification> _hubContext;
        private readonly ICurrentUserService<string> _currentUserService;

        public UsersController(IUserService userService, IHubContext<Notification> hubContext,
            ICurrentUserService<string> currentUserService)
        {
            _userService = userService;
            _hubContext = hubContext;
            _currentUserService = currentUserService;
        }


        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response);
        }


        [HttpGet]
        [AppAuthorize]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }



        [HttpGet]
        [AppAuthorize]
        public async Task<IActionResult> Private()
        {
            await _hubContext.Clients.User(_currentUserService.UserId).SendAsync("ReceiveMessage", "Server", "Message from the Server");
            return Ok();
        }

    }
}
