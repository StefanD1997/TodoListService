using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.BusinessLogic.Contracts;
using TodoList.BusinessLogic.Services;

namespace TodoList.BusinessLogic.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.ConfigureDI();

            return services;
        }

        private static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(TokenService))
                .AddScoped<IUserService, UserService>()
                .AddScoped<ITodoService, TodoService>();

            return services;
        }
    }
}
