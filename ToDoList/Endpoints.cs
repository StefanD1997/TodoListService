﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.DTOs;
using TodoList.Services;

namespace TodoList;

public static class Endpoint
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/User");

        group.MapPost("/Login",
        async ([FromBody] LoginDTO input, UserService service) =>
        {
            var token = await service.Login(input);
            return token is null? Results.NotFound() : Results.Ok(token);
        });

        group.MapPost("/Refresh",
        async (UserService service) =>
        {
            var token = await service.Refresh();
            return Results.Ok(token);
        });

        group.MapPost("/ChangePassword", [Authorize]
        async ([FromBody] ChangePasswordDTO input, UserService service) =>
        {
            var result = await service.ChangePassword(input);
            return result? Results.NoContent() : Results.BadRequest();
        });

        group.MapPost("/Create", [Authorize(Roles = Constants.ROLE_ADMIN)]
        async ([FromBody] CreateUserDTO input, UserService service) =>
        {
            var user = await service.Create(input);
            return user is null? Results.Conflict() : Results.Ok(user);
        });
    }

    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Todo");

        group.MapPost("/", [Authorize]
        async ([FromBody] CreateTodoDTO input, TodoService service) =>
        {
            var todo = await service.Create(input);
            return Results.Ok(todo);
        });

        group.MapPut("/Check/{id:int}", [Authorize]
        async (int id, TodoService service) =>
        {
            var todo = await service.Check(id);
            return todo is null? Results.NotFound() : Results.Ok(todo);
        });

        group.MapGet("/{id:int}", [Authorize]
        async (int id, TodoService service) =>
        {
            var todo = await service.Get(id);
            return Results.Ok(todo);
        });

        group.MapGet("/", [Authorize]
        async (TodoService service) =>
        {
            var todo = await service.Get();
            return Results.Ok(todo);
        });
    }
}