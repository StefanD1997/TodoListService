using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Models;
using TodoList.DataAccess.Repositories.Contracts;

namespace TodoList.DataAccess.Repositories
{
    public class TodoRepository(TodoListDBContext dBContext) : ITodoRepository
    {
        private readonly TodoListDBContext _dbContext = dBContext;

        public async Task Create(Todo newTodo)
        {
            await _dbContext.Todos.AddAsync(newTodo);
        }

        public void Delete(Todo deleteTodo)
        {
            _dbContext.Remove(deleteTodo);
        }

        public async Task<List<Todo>> GetAll()
            => await _dbContext.Todos.ToListAsync();

        public async Task<List<Todo>> GetAll(Expression<Func<Todo, bool>> predicate)
            => await _dbContext.Todos.Where(predicate).ToListAsync();

        public async Task<Todo?> GetById(int id)
        {
            Todo? foundTodo = await _dbContext.Todos.FindAsync(id);

            return foundTodo is null ? null : foundTodo;
        }

        public async Task<Todo?> GetFirstOrDefault(Expression<Func<Todo, bool>> predicate)
            => await _dbContext.Todos.Where(predicate).FirstOrDefaultAsync();

        public async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public void Update(Todo updateTodo)
        {
            _dbContext.Todos.Update(updateTodo);
        }
    }
}
