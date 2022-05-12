using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure.DbAccess;
using ToDoList.Infrastructure.Services.Authenticate;
using ToDoList.Infrastructure.Services.Tasks;

namespace ToDoList.WebApi.Ninject
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<AppDbContext>();

            Bind<IIdentityService>().To<IdentitytService>();
            Bind<ITaskListService>().To<TaskListService>();
            Bind<ITaskService>().To<TaskService>();
        }
    }
}
