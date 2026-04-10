using TodoServices.Dto;

namespace TodoServices.IService
    {
        public interface ITodoService
        {
            Task<List<TodoItem>> GetAllTodosAsync();
            Task<TodoItem> GetTodoByIdAsync(Guid id);
            Task CreateTodoItemAsync(TodoItem? todoItem);
            Task UpdateTodoItemAsync(Guid id, TodoItem? todoItem);
            Task DeleteTodoItemAsync(Guid id);
        }
    }
