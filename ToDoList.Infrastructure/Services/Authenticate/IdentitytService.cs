using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.JWT;
using ToDoList.Domain.Domain.Users;
using ToDoList.Domain.Domain.Users.Request;
using ToDoList.Domain.Domain.Users.Response;
using ToDoList.Infrastructure.DbAccess;

namespace ToDoList.Infrastructure.Services.Authenticate
{
    public class IdentitytService : IIdentityService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISecretService _secretService;

        public IdentitytService(AppDbContext dbContext, ISecretService secretService)
        {
            _dbContext = dbContext;
            _secretService = secretService;
        }

        public async Task<AuthenticatedUserResponse> LoginUser(UserLoginRequest user)
        {
            var userFound = await _dbContext.Users
                .FirstOrDefaultAsync(x => string.Equals(x.Username, user.Username));

            if(userFound == null)
            {
                return new AuthenticatedUserResponse 
                {
                    Success = false,
                    Errors = new [] { "This user and password combination does not exist" }
                };
            }

            var hashedPassword = _secretService.GeneratePasswordHash(user.Password);

            if(!string.Equals(userFound.Password, hashedPassword))
            {
                return new AuthenticatedUserResponse
                {
                    Success = false,
                    Errors = new[] { "This user and password combination does not exist" }
                };
            }

            var refreshToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.User.Id == userFound.Id && !x.Used);

            return await HandleUserTokens(userFound, refreshToken?.RToken ?? string.Empty);
        }

        public async Task<AuthenticatedUserResponse> LogoutUser(Guid userId)
        {
            if(string.IsNullOrEmpty(userId.ToString()))
            {
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "User Token is invalid!" }
                };
            }

            var userTokens = await _dbContext.RefreshTokens
                .Where(x => x.User.Id == userId).FirstOrDefaultAsync();

            if(userTokens is RefreshToken)
            {
                var result = _dbContext.RefreshTokens.Remove(userTokens);
                await _dbContext.SaveChangesAsync();
            }

            return new AuthenticatedUserResponse
            {
                Success = true,
                RefreshToken = string.Empty,
                Token = string.Empty
            };
        }

        public async Task<AuthenticatedUserResponse> RefreshUserToken(string token, string refreshToken)
        {
            if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "Jwt token or refreshToken are invalid" }
                };
            }

            var userRefreshToken = await _dbContext.RefreshTokens
                .Where(x => x.RToken == refreshToken).FirstOrDefaultAsync();

            if(userRefreshToken is not RefreshToken)
            {
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "Jwt token or refreshToken are invalid" }
                };
            }

            var isValid = await _secretService.GenerateJWTFromRefreshToken(token, refreshToken);

            return isValid;
        }

        public async Task<AuthenticatedUserResponse> RegisterUser(UserRegisterRequest user)
        {
            var exisiting = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (exisiting != null)
            {
                return new AuthenticatedUserResponse { 
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            // + 1 or more times
            Regex regex = new (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);

            if(!match.Success)
            {
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "Email address is invalid. Does not meet our requirments" }
                };
            }

            var hashedPassword = _secretService.GeneratePasswordHash(user.Password);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword,
                RegistrationDate = DateTime.UtcNow,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            var createdUser = await _dbContext.Users.AddAsync(newUser);

            if (createdUser == null)
            {
                return new AuthenticatedUserResponse { 
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            // Generate refresh Token also
            return await HandleUserTokens(createdUser.Entity, "");
        }

        public async Task<AuthenticatedUserResponse> HandleUserTokens(User user, string refreshToken)
        {
            var result = await _secretService.GenerateJWT(user, refreshToken);

            if (!result.Success)
            {
                return result;
            }

            return result;
        }
    }
}
