using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Tasks;
using ToDoList.Domain.Domain.Tasks.Request;
using ToDoList.Domain.Domain.Tasks.Response;
using ToDoList.Domain.Domain.Users;
using ToDoList.Infrastructure.DbAccess;

namespace ToDoList.Infrastructure.Services.Tasks
{
    public class TaskListService : ITaskListService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TaskListService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TaskListResponse> CreateTaskList(CreateTaskListRequest req, Guid userId)
        {
            var exisiting = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.User.Id == userId && x.Title == req.Title);

            if(exisiting != null)
            {
                return new TaskListResponse
                {
                    Success = false,
                    Errors = new[] { "This Task List already exists with this title" }
                };
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var taskList = _mapper.Map<ToDoTaskList>(req);
            taskList.Id = Guid.NewGuid();
            taskList.CreateDate = DateTime.UtcNow;
            taskList.User = user;

            await _dbContext.TaskLists.AddAsync(taskList);

            if (req.Tasks?.Count > 0)
            {
                var tasks = req.Tasks.Select(x =>
                {
                    var t = _mapper.Map<ToDoTask>(x);
                    t.Id = Guid.NewGuid();
                    t.CreateDate = DateTime.UtcNow;

                    return t;
                }).ToList();

                await _dbContext.Tasks.AddRangeAsync(tasks);
                taskList.Tasks = tasks;
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskListResponse>(taskList);
        }

        public async Task<TaskListResponse> DeleteTaskList(DeleteTaskListRequest req, Guid userId)
        {
            var exists = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.Id == req.TaskListId && x.User.Id == userId);

            if (exists == null)
            {
                return new TaskListResponse
                {
                    Success = false,
                    Errors = new[] { "This Task List doesn't exist or user is not allowed to delete it" }
                };
            }

            _dbContext.TaskLists.Remove(exists);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskListResponse>(exists);
        }

        public async Task<List<TaskListResponse>> ListAvailableTaskList(Guid userId)
        {
            var taskLists = await _dbContext.TaskLists
                .Where(x => x.User.Id == userId)
                .Include(t => t.Tasks)
                .ToListAsync();

            if (taskLists == null)
            {
                return new List<TaskListResponse>
                {
                    new TaskListResponse
                    {
                        Success = false,
                        Errors = new[] { "User does not have any available task lists" }
                    }
                };
            }

            return taskLists.Select(x => _mapper.Map<TaskListResponse>(x)).ToList();
        }

        public async Task<TaskListResponse> UpdateTaskList(UpdateTaskListRequest req, Guid userId)
        {
            var taskList = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.Id == req.TaskListId && x.User.Id == userId);

            if (taskList == null)
            {
                return new TaskListResponse
                {
                    Success = false,
                    Errors = new[] { "This Task List doesn't exists or user is not allowed to update it" }
                };
            }

            taskList.Title = req.Title;
            taskList.Description = req.Description;
            taskList.DueDate = req.DueDate;
            taskList.IsCompleted = req.IsCompleted;

            if(req.Tasks?.Count > 0)
            {
                foreach (var t in req.Tasks)
                {
                    var foundTask = taskList.Tasks
                        .FirstOrDefault(x => x.Id == t.TaskId);

                    foundTask = _mapper.Map<ToDoTask>(t);
                    foundTask.Id = t.TaskId == Guid.Empty ? Guid.NewGuid() : t.TaskId;

                    taskList.Tasks.Add(foundTask);
                }
            }

            _dbContext.TaskLists.Update(taskList);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TaskListResponse>(taskList);
        }

        public async Task<TaskListResponse> GetTaskList(Guid id, Guid userId)
        {
            var taskList = await _dbContext.TaskLists
                .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == userId);

            if (taskList == null)
            {
                return new TaskListResponse
                {
                    Success = false,
                    Errors = new[] { "This Task List doesn't exists or user is not allowed to update it" }
                };
            }

            var tasks = await _dbContext.Tasks
                .Where(x => x.TaskList.Id == id).ToListAsync();

            return _mapper.Map<TaskListResponse>(taskList); 
        }
    }
}
