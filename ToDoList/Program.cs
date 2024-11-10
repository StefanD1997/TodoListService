using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TodoList;
using TodoList.BusinessLogic.Extensions;
using TodoList.Core.Common.Contracts;
using TodoList.DataAccess.ServiceExtensions;
using TodoList.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddBusiness();

builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();
