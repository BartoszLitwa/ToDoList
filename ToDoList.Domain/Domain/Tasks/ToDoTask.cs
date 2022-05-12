using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Domain.Domain.Tasks
{
    public class ToDoTask
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("ToDoTaskListId")]
        public ToDoTaskList TaskList { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
