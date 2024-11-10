using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Models;

namespace TodoList.DataAccess.Repositories.Contracts
{
    public interface ITodoRepository
    {
        Task Create(Todo newTodo);
        void Update(Todo updateTodo);
        void Delete(Todo deleteTodo);
        Task<List<Todo>> GetAll();
        Task<List<Todo>> GetAll(Expression<Func<Todo, bool>> predicate);
        Task<Todo?> GetById(int id);
        Task<Todo?> GetFirstOrDefault(Expression<Func<Todo, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
