using TodoRepositories.Entity;

namespace TodoRepositories.IRepository
    {
        public interface ITodoRepository
        {
            Task<List<TodoItemEntity>> GetAllTodos();
            Task<TodoItemEntity?> GetTodoById(Guid id);
            Task CreateTodoItemAsync(TodoItemEntity todoItem);
            Task UpdateTodoItemAsync(TodoItemEntity todoItem);
            Task DeleteTodoItemAsync(Guid id);
        }
    }
