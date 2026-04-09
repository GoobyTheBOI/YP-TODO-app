using Microsoft.EntityFrameworkCore;
using TodoRepositories.Entity;

namespace TodoRepositories
{
    public class TodoRepository(TodoDbContext dbContext)
    {
        public async Task<List<TodoItemEntity>> getAllTodos()
        {
            return await dbContext.Todos.ToListAsync();
        }

        public async Task<TodoItemEntity?> getTodoById(Guid id)
        {
            return await dbContext.Todos.FindAsync(id);
        }

        public async Task createTodoItemAsync(TodoItemEntity todoItem)
        {
            dbContext.Todos.Add(todoItem);
            await dbContext.SaveChangesAsync();
        }

        public async Task updateTodoItemAsync(TodoItemEntity todoItem)
        {
            dbContext.Todos.Update(todoItem);
            await dbContext.SaveChangesAsync();
        }

        public async Task deleteTodoItemAsync(Guid id)
        {
            await dbContext.Todos.Where(t => t.Id == id).ExecuteDeleteAsync();
        }
    }
}
