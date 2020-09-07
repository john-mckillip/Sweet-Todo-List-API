using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListAPI.Contexts;
using TodoListAPI.Entities;
using TodoListAPI.Services.Abstractions;

namespace TodoListAPI.Services
{
    public class TodoListInfoRepository : ITodoListInfoRepository
    {
        private readonly TodoInfoContext _context;

        public TodoListInfoRepository(
            TodoInfoContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateTodo(Todo todo)
        {
            _context.Todos.Add(todo);

            return  await Save();
        }

        public void DeleteTodo(Todo todo)
        {
            _context.Todos.Remove(todo);
        }

        public async Task<Todo> GetTodo(Guid id)
        {
            return await _context.Todos.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Todo>> GetTodos()
        {
            return await _context.Todos.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var success = await _context.SaveChangesAsync();
            return (success >= 0);
        }

        public async Task<bool> TodoExists(Guid todoId)
        {
            return await _context.Todos.AnyAsync(c => c.Id == todoId);
        }

        public void UpdateTodo(Guid todoId, Todo todo)
        {
            // Since my implementation of ITodoListInfoRepository is using EF Core to track entities,
            // This method is not being used. I am leaving this in the interface to make this more stable for 
            // other implementations that may not be using an ORM to track entities and will ned to implement this method.
        }
    }
}
