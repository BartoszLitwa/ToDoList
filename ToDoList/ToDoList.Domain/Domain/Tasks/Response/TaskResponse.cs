﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Domain.Tasks.Response
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool Success { get; set; } = true;
        public IEnumerable<string> Errors { get; set; }
    }
}
