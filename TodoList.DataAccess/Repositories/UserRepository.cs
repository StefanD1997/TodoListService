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
    public class UserRepository(TodoListDBContext todoListDBContext) : IUserRepository
    {
        private readonly TodoListDBContext _dbContext = todoListDBContext;

        public async Task Create(User newUser)
        {
            await _dbContext.Users.AddAsync(newUser);
        }

        public void Delete(User deleteUser)
        {
            _dbContext.Remove(deleteUser);
        }

        public async Task<List<User>> GetAll()
            => await _dbContext.Users.ToListAsync();

        public async Task<List<User>> GetAll(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.Where(predicate).ToListAsync();

        public async Task<User?> GetById(int id)
        {
            User? foundUser = await _dbContext.Users.FindAsync(id);

            return foundUser is null ? null : foundUser;
        }

        public async Task<User?> GetFirstOrDefault(Expression<Func<User, bool>> predicate)
            => await _dbContext.Users.Where(predicate).FirstOrDefaultAsync();

        public async Task<int> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public void Update(User updateUser)
        {
            _dbContext.Users.Update(updateUser);
        }
    }
}
