using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.DataAccess.Repositories;
using TodoList.DataAccess.Repositories.Contracts;

namespace TodoList.DataAccess.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDbContext(configuration);

            services.ConfigureDI();

            return services;
        }

        private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoListDBContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

        private static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services
                .AddScoped<ITodoRepository, TodoRepository>()
                .AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
