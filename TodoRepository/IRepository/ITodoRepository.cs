using TodoRepositories.Entity;

namespace TodoRepositories.IRepository
{
    public interface ITodoRepository
    {
        Task<List<TodoItemEntity>> getAllTodos();
        Task<TodoItemEntity?> getTodoById(Guid id);
        Task createTodoItemAsync(TodoItemEntity todoItem);
        Task updateTodoItemAsync(TodoItemEntity todoItem);
        Task deleteTodoItemAsync(Guid id);
    }
}
