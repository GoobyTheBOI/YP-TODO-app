using TodoServices.Dto;
using TodoRepositories;
using TodoRepositories.Entity;
using TodoRepositories.IRepository;
using TodoServices.Exceptions;
using TodoServices.IService;

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

        public async Task<TodoItem> GetTodoByIdAsync(Guid id) {

            if(id.Equals(Guid.Empty))
            {
                throw new NotFoundException($"Todo item with id {id} was not found.");
            }

            var result = await todoRepository.GetTodoById(id);

            if (result == null)
            {
                throw new NotFoundException($"Todo item with id {id} was not found.");
            }

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
                return;
            }

            var entity = new TodoItemEntity
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                isCompleted = todoItem.isCompleted,
                CreatedDate = todoItem.CreatedDate
            };

            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                throw new MissingDataException("Todo item title is missing.");
            }

            await todoRepository.CreateTodoItemAsync(entity);
        }

        public async Task UpdateTodoItemAsync(Guid id, TodoItem? todoItem)
        {
            if (todoItem == null)
            {
                throw new MissingDataException("Todo item data are required to update a todo item.");
            }

            if (id == Guid.Empty)
            {
                throw new NotFoundException($"Todo item with id {id} was not found.");
            }

            var entity = new TodoItemEntity
            {
                Id = id,
                Title = todoItem.Title,
                isCompleted = todoItem.isCompleted,
                CreatedDate = todoItem.CreatedDate
            };

            await todoRepository.UpdateTodoItemAsync(entity);
        }

        public async Task DeleteTodoItemAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new MissingDataException("Id is required to delete a todo item.");
            }

            var task = await todoRepository.GetTodoById(id);

            if(task == null)
            {
                throw new NotFoundException($"Todo item with id {id} was not found.");
            }

            await todoRepository.DeleteTodoItemAsync(id);
        }

    }
}
