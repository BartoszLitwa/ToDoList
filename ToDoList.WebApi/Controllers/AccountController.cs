using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoList.Domain.Api.V1;
using ToDoList.Infrastructure.Services.Account;

namespace ToDoList.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;

        public AccountController(IAccountService account)
        {
            _account = account;
        }

        [HttpGet(Routes.V1.Account.GET)]
        public async Task<string> Get()
        {
            return nameof(AccountController);
        }
    }
}
