using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Users;
using ToDoList.Domain.Domain.Users.Request;
using ToDoList.Domain.Domain.Users.Response;

namespace ToDoList.Infrastructure.Services.Authenticate
{
    public interface IIdentityService
    {
        Task<AuthenticatedUserResponse> LoginUser(UserLoginRequest user);
        Task<AuthenticatedUserResponse> LogoutUser(Guid userId);
        Task<AuthenticatedUserResponse> RegisterUser(UserRegisterRequest user);
        Task<AuthenticatedUserResponse> RefreshUserToken(string token, string refreshToken);
        Task<AuthenticatedUserResponse> HandleUserTokens(User user, string refreshToken = "");
    }
}
