using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.Tasks;
using ToDoList.Domain.Domain.Tasks.Request;
using ToDoList.Domain.Domain.Tasks.Request.Task;
using ToDoList.Domain.Domain.Users;
using ToDoList.Domain.Domain.Users.Request;

namespace ToDoList.WebApi.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CreateTaskRequest, ToDoTask>();
            CreateMap<DeleteTaskRequest, ToDoTask>();
            //CreateMap<GetTaskFromTaskListRequest, ToDoTask>();
            CreateMap<ToggleCompletionTaskRequest, ToDoTask>();
            CreateMap<UpdateTaskRequest, ToDoTask>();

            CreateMap<CreateTaskListRequest, ToDoTaskList>();
            CreateMap<UpdateTaskListRequest, ToDoTaskList>();
            CreateMap<DeleteTaskRequest, ToDoTaskList>();
            CreateMap<UpdateTaskRequest, ToDoTaskList>();

            CreateMap<UserLoginRequest, User>();
            CreateMap<UserRegisterRequest, User>();
        }
    }
}
