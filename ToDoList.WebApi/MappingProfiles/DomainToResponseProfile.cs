using AutoMapper;
using ToDoList.Domain.Domain.Tasks;
using ToDoList.Domain.Domain.Tasks.Response;

namespace ToDoList.WebApi.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<ToDoTask, TaskResponse>();
            CreateMap<ToDoTask, TaskDeletedResponse>();

            CreateMap<ToDoTaskList, TaskListResponse>()
                .ForMember(dest => dest.Tasks, opt =>
                {
                    opt.MapFrom(src => src.Tasks);
                });
        }
    }
}
