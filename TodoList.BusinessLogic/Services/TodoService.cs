using Microsoft.EntityFrameworkCore;
using TodoList.DTOs.Todo;
using TodoList.BusinessLogic.Contracts;
using TodoList.Core.Models;
using TodoList.DataAccess.Repositories.Contracts;
using TodoList.Core.Common.Contracts;

namespace TodoList.BusinessLogic.Services;

public class TodoService(
    ITodoRepository todoRepository,
    ICurrentUser currentUser) : ITodoService
{
    private readonly ITodoRepository _todoRepository = todoRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Todo?> Create(CreateTodoDTO input)
    {
        var todo = Todo.Create(int.Parse(_currentUser.Id), input.Title, input.Description);

        await _todoRepository.Create(todo);
        await _todoRepository.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo?> Check(int id)
    {
        Todo? todo = await _todoRepository.GetFirstOrDefault(x => 
                x.Id == id && 
                x.UserId == int.Parse(_currentUser.Id)
            );

        if (todo is null)
        {
            return null;
        }

        todo.IsDone = true;

        _todoRepository.Update(todo);
        await _todoRepository.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo?> Get(int id)
    {
        Todo? todo = await _todoRepository.GetFirstOrDefault(x =>
            x.Id == id &&
            x.UserId == int.Parse(_currentUser.Id));

        if (todo is null)
        {
            return null;
        }

        return todo;
    }

    public async Task<List<Todo>> Get()
    {
        List<Todo> todos = await _todoRepository.GetAll(x => x.UserId == int.Parse(_currentUser.Id));

        return todos;
    }

    public async Task<Todo?> Update(UpdateTodoDTO input)
    {
        var todo = await _todoRepository.GetById(input.Id);

        if (todo is null)
        {
            return null;
        }

        todo.Description = input.Description;
        todo.Id = input.Id;
        todo.Title = input.Title;
        todo.IsDone = input.IsDone;

        await _todoRepository.SaveChangesAsync();

        return todo;
    }

    public async Task<bool> Delete(int id)
    {
        var todo = await _todoRepository.GetById(id);

        if (todo is null)
        {
            return false;
        }

        _todoRepository.Delete(todo);
        await _todoRepository.SaveChangesAsync();

        return true;
    }
}
