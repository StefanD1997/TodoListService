using Microsoft.EntityFrameworkCore;
using TodoList.Core.Models;

namespace TodoList.DataAccess;

public class TodoListDBContext(DbContextOptions<TodoListDBContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(User.CreateAdmin());
    }
}