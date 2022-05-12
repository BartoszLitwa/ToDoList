using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Tasks.Request.Task;
using ToDoList.Domain.Domain.Tasks.Response;

namespace ToDoList.Infrastructure.Services.Tasks
{
    public interface ITaskService
    {
        Task<List<TaskResponse>> ListTaskInTaskList(Guid taskListId);
        Task<TaskResponse> CreateTask(CreateTaskRequest request);
        Task<TaskResponse> UpdateTask(UpdateTaskRequest request);
        Task<TaskDeletedResponse> DeleteTask(DeleteTaskRequest request);
        Task<TaskResponse> ToggleCompletionTask(ToggleCompletionTaskRequest request);
        Task<TaskResponse> GetTaskInTaskList(GetTaskFromTaskListRequest request);
        Task<TaskResponse> ToggleTaskStatus(Guid taskId);
    }
}
