using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Users;
using ToDoList.Domain.Domain.Users.Response;

namespace ToDoList.Infrastructure.Services.Authenticate
{
    public interface ISecretService
    {
        Task<AuthenticatedUserResponse> GenerateJWT(User user, string refreshToken = "");
        Task<AuthenticatedUserResponse> GenerateRefreshToken(AuthenticatedUserResponse response, User user);
        Task<AuthenticatedUserResponse> GenerateJWTFromRefreshToken(string token, string refreshToken);
        string GeneratePasswordHash(string password);
    }
}
