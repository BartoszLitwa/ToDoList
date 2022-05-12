using System;
using System.Collections.Generic;
using ToDoList.Domain.Domain.Tasks;

namespace ToDoList.Domain.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public IList<ToDoTaskList> Items { get; set; }
    }
}
