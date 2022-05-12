using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Infrastructure.Services.Account;
using ToDoList.Infrastructure.Services.Authenticate;
using ToDoList.Infrastructure.Services.Tasks;

namespace ToDoList.WebApi.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentitytService>();

            services.AddScoped<ITaskListService, TaskListService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ISecretService, SecretService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddAutoMapper(typeof(Startup));
        }
    }
}
