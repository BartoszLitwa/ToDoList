using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Services.Authenticate;

namespace ToDoList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISecretService _secretService;

        public HomeController(ISecretService secretService)
        {
            _secretService = secretService;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { _secretService.GeneratePasswordHash("test"), _secretService.GeneratePasswordHash("test") };
        }
    }
}
