using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Users;

namespace ToDoList.Infrastructure.Services.Account
{
    public interface IAccountService
    {
        Task<User> GetUserInformation(Guid userId);
        Task<User> ChangeUsername(Guid userId, string username);
        Task<User> ChangeEmail(Guid userId, string oldEmail, string newEmail);
        Task<User> ChangePassword(Guid userId, string oldPassword, string newPassword);
        Task<User> DeleteAccount(Guid userId, string emal, string password);
    }
}
