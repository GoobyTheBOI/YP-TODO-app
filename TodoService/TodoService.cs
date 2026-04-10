using TodoServices.Dto;
using TodoRepositories.Entity;
using TodoRepositories.IRepository;
using TodoServices.Exceptions;
using TodoServices.IService;
using TodoServices.ValueObjects;

namespace TodoServices.Service
{
    public class TodoService(ITodoRepository todoRepository) : ITodoService
    {
        public async Task<List<TodoItem>> GetAllTodosAsync()
        {
            var result = await todoRepository.GetAllTodos();

            var todoItems = result.Select(todoItem => new TodoItem
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                isCompleted = todoItem.isCompleted,
                CreatedDate = todoItem.CreatedDate
            }).ToList();

            return todoItems;
        }

        public async Task<TodoItem> GetTodoByIdAsync(Guid id)
        {
            var validatedId = TodoId.Create(id);

            var result = await todoRepository.GetTodoById(validatedId.Value) ?? 
                throw new NotFoundException($"Todo item with id {validatedId} was not found.");

            var todoItem = new TodoItem
            {
                Id = result.Id,
                Title = result.Title,
                isCompleted = result.isCompleted,
                CreatedDate = result.CreatedDate
            };

            return todoItem;
        }

        public async Task CreateTodoItemAsync(TodoItem? todoItem)
        {
            if (todoItem == null)
            {
                throw new BadRequestException("Todo item cannot be null.");
            }

            var validatedTitle = TodoItemTitle.Create(todoItem.Title);

            var entity = new TodoItemEntity
            {
                Id = todoItem.Id,
                Title = validatedTitle.Value,
                isCompleted = todoItem.isCompleted,
                CreatedDate = todoItem.CreatedDate
            };

            await todoRepository.CreateTodoItemAsync(entity);
        }

        public async Task UpdateTodoItemAsync(Guid id, TodoItem? todoItem)
        {
            var validatedId = TodoId.Create(id);

            if (todoItem == null)
            {
                throw new BadRequestException("Todo item data are required to update a todo item.");
            }

            var validatedTitle = TodoItemTitle.Create(todoItem.Title);

            var entity = new TodoItemEntity
            {
                Id = validatedId.Value,
                Title = validatedTitle.Value,
                isCompleted = todoItem.isCompleted,
                CreatedDate = todoItem.CreatedDate
            };

            await todoRepository.UpdateTodoItemAsync(entity);
        }

        public async Task DeleteTodoItemAsync(Guid id)
        {
            var validatedId = TodoId.Create(id);

            var task = await todoRepository.GetTodoById(validatedId.Value) ??
                throw new NotFoundException($"Todo item with id {validatedId} was not found.");

            await todoRepository.DeleteTodoItemAsync(validatedId.Value);
        }

    }
}
