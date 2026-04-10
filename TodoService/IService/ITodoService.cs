using TodoServices.Dto;

namespace TodoServices.IService
{
    public interface ITodoService
    {
        Task<List<TodoItem>> getAllTodosAsync();
        Task<TodoItem> getTodoByIdAsync(Guid id);
        Task createTodoItemAsync(TodoItem? todoItem);
        Task updateTodoItemAsync(Guid id, TodoItem? todoItem);
        Task deleteTodoItemAsync(Guid id);
    }
}
