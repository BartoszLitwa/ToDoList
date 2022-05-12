using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Users;
using ToDoList.Infrastructure.DbAccess;

namespace ToDoList.Infrastructure.Services.Helpers
{
    public static class UserHelpers
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.Claims.ElementAt(4).Value;
            return new Guid(userId);
        }
    }
}
