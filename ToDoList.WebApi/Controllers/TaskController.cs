using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Api.V1;
using ToDoList.Domain.Domain.Tasks.Request.Task;
using ToDoList.Domain.Domain.Tasks.Response;
using ToDoList.Domain.Domain.Users;
using ToDoList.Infrastructure.Services.Tasks;

namespace ToDoList.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost(Routes.V1.Task.CREATE)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var response = await _taskService.CreateTask(request);

            return HandleTaskListResponse(response);
        }

        // get/{id}
        [HttpGet(Routes.V1.Task.GET)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _taskService.ListTaskInTaskList(id);

            return Ok(response);
        }

        [HttpGet(Routes.V1.Task.GET_TASK)]
        public async Task<IActionResult> Get([FromBody] GetTaskFromTaskListRequest request)
        {
            var response = await _taskService.GetTaskInTaskList(request);

            return HandleTaskListResponse(response);
        }

        [HttpPut(Routes.V1.Task.UPDATE)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskRequest request)
        {
            var response = await _taskService.UpdateTask(request);

            return HandleTaskListResponse(response);
        }

        [HttpDelete(Routes.V1.Task.DELETE)]
        public async Task<IActionResult> Delete([FromBody] DeleteTaskRequest request)
        {
            var response = await _taskService.DeleteTask(request);

            return response.Success ? NoContent() : Conflict(response);
        }

        // toggle/{taskId}
        [HttpPatch(Routes.V1.Task.TOGGLE)]
        public async Task<IActionResult> Toggle([FromBody] ToggleCompletionTaskRequest request)
        {
            var response = await _taskService.ToggleCompletionTask(request);

            return HandleTaskListResponse(response);
        }

        private IActionResult HandleTaskListResponse(TaskResponse response)
        {
            return response.Success ? Ok(response) : BadRequest(new TaskErrorResponse
            {
                Success = response.Success,
                Errors = response.Errors
            });
        }
    }
}
