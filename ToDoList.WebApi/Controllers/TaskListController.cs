using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToDoList.Domain.Api.V1;
using ToDoList.Domain.Domain.Tasks.Request;
using ToDoList.Domain.Domain.Tasks.Response;
using ToDoList.Domain.Domain.Users;
using ToDoList.Infrastructure.Services.Helpers;
using ToDoList.Infrastructure.Services.Tasks;

namespace ToDoList.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskListService _taskList;

        public TaskListController(ITaskListService taskList)
        {
            _taskList = taskList;
        }

        [HttpPost(Routes.V1.TaskList.CREATE)]
        public async Task<IActionResult> CreateTaskList([FromBody] CreateTaskListRequest request)
        {
            var userId = User.GetUserId();
            var response = await _taskList.CreateTaskList(request, userId);

            return HandleTaskListResponse(response);
        }

        [HttpGet(Routes.V1.TaskList.LIST)]
        public async Task<IActionResult> ListAvailableTask()
        {
            var userId = User.GetUserId();
            var response = await _taskList.ListAvailableTaskList(userId);

            return response != null ? Ok(response) : BadRequest(response);
        }

        // get/{id}
        [HttpGet(Routes.V1.TaskList.GET)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var userId = User.GetUserId();
            var response = await _taskList.GetTaskList(new Guid(id), userId);

            return HandleTaskListResponse(response);
        }

        [HttpPut(Routes.V1.TaskList.UPDATE)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskListRequest request)
        {
            var userId = User.GetUserId();
            var response = await _taskList.UpdateTaskList(request, userId);

            return HandleTaskListResponse(response);
        }

        [HttpDelete(Routes.V1.TaskList.DELETE)]
        public async Task<IActionResult> Delete([FromBody] DeleteTaskListRequest request)
        {
            var userId = User.GetUserId();
            var response = await _taskList.DeleteTaskList(request, userId);

            return HandleTaskListResponse(response);
        }

        private IActionResult HandleTaskListResponse(TaskListResponse response)
        {
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
