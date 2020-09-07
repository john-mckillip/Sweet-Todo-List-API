using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListAPI.Entities;

namespace TodoListAPI.Services.Abstractions
{
    public interface ITodoListInfoRepository
    {
        Task<bool> CreateTodo(Todo todo);

        void DeleteTodo(Todo todo);

        Task<Todo> GetTodo(Guid todoId);

        Task<IEnumerable<Todo>> GetTodos();

        Task<bool> Save();

        Task<bool> TodoExists(Guid todoId);

        void UpdateTodo(Guid todoId, Todo todo);        
    }
}
