using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Domain.Domain.Users;

namespace ToDoList.Domain.Domain.Tasks
{
    public class ToDoTaskList
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public IList<ToDoTask> Tasks { get; set; }
    }
}
