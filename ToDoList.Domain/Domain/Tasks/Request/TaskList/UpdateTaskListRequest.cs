﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Tasks.Request.Task;

namespace ToDoList.Domain.Domain.Tasks.Request
{
    public class UpdateTaskListRequest
    {
        public Guid TaskListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public IList<UpdateTaskRequest> Tasks { get; set; }
    }
}
