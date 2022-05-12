using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Api.V1;
using ToDoList.Domain.Domain.JWT.Request;
using ToDoList.Domain.Domain.Users.Request;
using ToDoList.Domain.Domain.Users.Response;
using ToDoList.Infrastructure.Services.Authenticate;
using ToDoList.Infrastructure.Services.Helpers;

namespace ToDoList.WebApi.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identity;

        public IdentityController(IIdentityService authenticate)
        {
            _identity = authenticate;
        }

        [HttpPost(Routes.V1.Identity.LOGIN)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values
                    .SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
            }

            var authResponse = await _identity.LoginUser(user);

            return HandleAuthResponse(authResponse);
        }

        [HttpGet(Routes.V1.Identity.LOGOUT)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            var userId = User.GetUserId();
            var authResponse = await _identity.LogoutUser(userId);

            return HandleAuthResponse(authResponse);
        }

        [HttpPost(Routes.V1.Identity.REGISTER)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values
                    .SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
            }

            var authResponse = await _identity.RegisterUser(user);

            return HandleAuthResponse(authResponse);
        }

        [HttpPost(Routes.V1.Identity.REFRESH)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest tokens)
        {
            var authResponse = await _identity.RefreshUserToken(tokens.Token, tokens.RefreshToken);

            return HandleAuthResponse(authResponse);
        }

        private IActionResult HandleAuthResponse(AuthenticatedUserResponse authResponse)
        {
            if (authResponse == null || !authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors,
                });
            }

            return Ok(authResponse);
        }
    }
}
