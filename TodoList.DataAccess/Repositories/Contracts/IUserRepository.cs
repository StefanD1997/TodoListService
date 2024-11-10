using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Models;

namespace TodoList.DataAccess.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task Create(User newUser);
        void Update(User updateUser);
        void Delete(User deleteUser);
        Task<List<User>> GetAll();
        Task<List<User>> GetAll(Expression<Func<User, bool>> predicate);
        Task<User?> GetById(int id);
        Task<User?> GetFirstOrDefault(Expression<Func<User, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
