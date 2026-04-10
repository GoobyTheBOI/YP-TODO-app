using Microsoft.EntityFrameworkCore;
using TodoRepositories.Entity;
using TodoRepositories.IRepository;

namespace TodoRepositories
{
    public class TodoRepository(TodoDbContext dbContext) : ITodoRepository
    {
        public async Task<List<TodoItemEntity>> GetAllTodos()
        {
            return await dbContext.Todos.ToListAsync();
        }

        public async Task<TodoItemEntity?> GetTodoById(Guid id)
        {
            return await dbContext.Todos.FindAsync(id);
        }

        public async Task CreateTodoItemAsync(TodoItemEntity todoItem)
        {
            dbContext.Todos.Add(todoItem);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateTodoItemAsync(TodoItemEntity todoItem)
        {
            dbContext.Todos.Update(todoItem);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteTodoItemAsync(Guid id)
        {
            await dbContext.Todos.Where(t => t.Id == id).ExecuteDeleteAsync();
        }
    }
}
