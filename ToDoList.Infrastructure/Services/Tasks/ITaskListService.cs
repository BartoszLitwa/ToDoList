using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Tasks;
using ToDoList.Domain.Domain.Tasks.Request;
using ToDoList.Domain.Domain.Tasks.Response;
using ToDoList.Domain.Domain.Users;

namespace ToDoList.Infrastructure.Services.Tasks
{
    public interface ITaskListService
    {
        Task<TaskListResponse> CreateTaskList(CreateTaskListRequest req, Guid userId);
        Task<List<TaskListResponse>> ListAvailableTaskList(Guid userId);
        Task<TaskListResponse> UpdateTaskList(UpdateTaskListRequest req, Guid userId);
        Task<TaskListResponse> DeleteTaskList(DeleteTaskListRequest req, Guid userId);
        Task<TaskListResponse> GetTaskList(Guid id, Guid userId);
    }
}
