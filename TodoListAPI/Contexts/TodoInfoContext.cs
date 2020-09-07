using Microsoft.EntityFrameworkCore;
using TodoListAPI.Entities;

namespace TodoListAPI.Contexts
{
    public class TodoInfoContext : DbContext
    {
        public DbSet<Todo>Todos { get; set; }

        public TodoInfoContext(DbContextOptions<TodoInfoContext> options)
            : base(options)
        {
        }
    }
}
