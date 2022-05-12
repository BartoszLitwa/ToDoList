using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Domain.Tasks.Request.Task
{
    public class ToggleCompletionTaskRequest
    {
        public Guid TaskId { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
