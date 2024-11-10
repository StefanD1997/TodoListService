using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TodoList.BusinessLogic.Services;
using TodoList.Core.Common.Contracts;
using TodoList.Core.Common.IOptions;

namespace TodoList.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureIOptions(configuration);

            services.ConfigureJwtAuthentication(
                services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>()
                );

            services.ConfigureSwaggerGen();

            services.ConfigureDI();

            return services;
        }

        private static IServiceCollection ConfigureIOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }

        private static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IOptions<JwtSettings> jwtSettings)
        {
            JwtSettings _jwtSettings = jwtSettings.Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _jwtSettings.Issuer,
                        ValidAudience = _jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
                    };
                });

            return services;
        }

        private static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo
                 {
                     Title = "Swagger",
                     Contact = new OpenApiContact
                     {
                         Name = "Behzad Dara",
                         Email = "Behzad.Dara.99@gmail.com",
                         Url = new Uri("https://www.linkedin.com/in/behzaddara/")
                     }
                 });

                 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     In = ParameterLocation.Header,
                     Description = "Please insert JWT into field",
                     Name = "Authorization",
                     Type = SecuritySchemeType.Http,
                     BearerFormat = "JWT",
                     Scheme = "bearer"
                 });

                 c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
             });

            return services;
        }

        private static IServiceCollection ConfigureDI(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUser, CurrentUser>();

            return services;
        }
    }
}
