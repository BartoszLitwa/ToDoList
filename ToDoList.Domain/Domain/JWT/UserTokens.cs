using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Users;

namespace ToDoList.Domain.Domain.JWT
{
    public class UserTokens
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
