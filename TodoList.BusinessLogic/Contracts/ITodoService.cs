using TodoList.DTOs.Todo;
using TodoList.Core.Models;

namespace TodoList.BusinessLogic.Contracts;

public interface ITodoService
{
    Task<Todo?> Create(CreateTodoDTO input);
    Task<Todo?> Check(int id);
    Task<Todo?> Get(int id);
    Task<List<Todo>> Get();
    Task<Todo?> Update(UpdateTodoDTO input);
    Task<bool> Delete(int id);
}

