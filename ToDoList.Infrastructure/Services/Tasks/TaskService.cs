using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Tasks;
using ToDoList.Domain.Domain.Tasks.Request.Task;
using ToDoList.Domain.Domain.Tasks.Response;
using ToDoList.Infrastructure.DbAccess;

namespace ToDoList.Infrastructure.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TaskService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TaskResponse> CreateTask(CreateTaskRequest request)
        {
            var taskList = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.Id == request.TaskListId);

            if (taskList != null && taskList.Tasks?.Count > 0)
            {
                var foundTask = taskList.Tasks.FirstOrDefault(x => x.Title == request.Title);

                if(foundTask is ToDoTask)
                {
                    return new TaskResponse
                    {
                        Success = false,
                        Errors = new[] { "Task with the same title already exists" }
                    };
                }
            }

            var newTask = _mapper.Map<ToDoTask>(request);
            newTask.TaskList = taskList;
            newTask.Id = Guid.NewGuid();
            newTask.CreateDate = DateTime.UtcNow;

            await _dbContext.AddAsync(newTask);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskResponse>(newTask);
        }

        public async Task<TaskDeletedResponse> DeleteTask(DeleteTaskRequest request)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId);

            if (task is null)
            {
                return new TaskDeletedResponse
                {
                    Success = false,
                    Errors = new[] { "This Task does not exist or you do not have permissions to delete it" }
                };
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return new TaskDeletedResponse
            {
                Success = true
            };
        }

        public async Task<TaskResponse> GetTaskInTaskList(GetTaskFromTaskListRequest request)
        {
            var task = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.Id == request.TaskListId &&
                x.Tasks.FirstOrDefault(y => y.Id == request.TaskId) != default(ToDoTask));

            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<List<TaskResponse>> ListTaskInTaskList(Guid taskListId)
        {
            var taskList = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.Id == taskListId);

            return _mapper.Map<List<TaskResponse>>(taskList);
        }

        public async Task<TaskResponse> ToggleCompletionTask(ToggleCompletionTaskRequest request)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId);

            if (task is null)
            {
                return new TaskResponse
                {
                    Success = false,
                    Errors = new[] { "This Task does not exist or you do not have permissions" }
                };
            }

            task.IsCompleted = request.CompletedDate != null;
            if (task.IsCompleted)
                task.CompletedDate = request.CompletedDate;
            else
                task.CompletedDate = null;

            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse> UpdateTask(UpdateTaskRequest request)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId);

            if (task is null)
            {
                return new TaskResponse
                {
                    Success = false,
                    Errors = new[] { "This Task does not exist or you do not have permissions" }
                };
            }

            if (!string.IsNullOrEmpty(request.Title))
                task.Title = request.Title;
            if (!string.IsNullOrEmpty(request.Description))
                task.Description = request.Description;

            if(request.DueDate != null)
                task.DueDate = request.DueDate;

            if(request.CompletedDate != null)
                task.CompletedDate = request.CompletedDate;

            task.IsCompleted = request.IsCompleted;

            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse> ToggleTaskStatus(Guid taskId)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task is null)
            {
                return new TaskResponse
                {
                    Success = false,
                    Errors = new[] { "This Task does not exist or you do not have permissions" }
                };
            }

            task.IsCompleted = !task.IsCompleted;

            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskResponse>(task);
        }
    }
}
