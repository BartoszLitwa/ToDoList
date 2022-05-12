using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Users;
using ToDoList.Infrastructure.DbAccess;
using ToDoList.Infrastructure.Exceptions.Account;
using ToDoList.Infrastructure.Services.Authenticate;

namespace ToDoList.Infrastructure.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISecretService _secretService;

        public AccountService(AppDbContext dbContext, ISecretService secretService)
        {
            _dbContext = dbContext;
            _secretService = secretService;
        }

        public async Task<User> ChangeEmail(Guid userId, string oldEmail, string newEmail)
        {
            var user = await GetUser(userId);

            if (user is not User)
            {
                throw new UserNotFoundException();
            }

            if(user.Email != oldEmail)
            {
                throw new UserEmailNotMatchedException();
            }

            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);

            if (!match.Success)
            {
                throw new UserEmailNotMetRequirments();
            }

            user.Email = newEmail;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await GetUser(userId);

            if (user is not User)
            {
                throw new UserNotFoundException();
            }

            var hashedPass = _secretService.GeneratePasswordHash(oldPassword);
            if (!string.Equals(user.Password, hashedPass))
            {
                throw new UserEmailNotMatchedException();
            }

            var newHashedPass = _secretService.GeneratePasswordHash(newPassword);

            user.Password = newHashedPass;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> ChangeUsername(Guid userId, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> DeleteAccount(Guid userId, string emal, string password)
        {
            var user = await GetUser(userId);

            if (user is not User)
            {
                throw new UserNotFoundException();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            throw new UserHasBeenDeletedException();
        }

        public async Task<User> GetUserInformation(Guid userId)
        {
            var user = await GetUser(userId);

            if (user is not User)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        private async Task<User> GetUser(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
