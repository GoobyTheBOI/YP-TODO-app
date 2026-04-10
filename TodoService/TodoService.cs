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
        public async Task<List<TodoItem>> getAllTodosAsync()
        {
            var result = await todoRepository.getAllTodos();

            var todoItems = result.Select(todoItem => new TodoItem
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                isCompleted = todoItem.isCompleted,
                CreatedDate = todoItem.CreatedDate
            }).ToList();

            return todoItems;
        }

        public async Task<TodoItem> getTodoByIdAsync(Guid id) {

            if(id.Equals(Guid.Empty))
            {
                throw new NotFoundException($"Todo item with id {id} was not found.");
            }

            var result = await todoRepository.getTodoById(id);

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

        public async Task createTodoItemAsync(TodoItem? todoItem)
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

            await todoRepository.createTodoItemAsync(entity);
        }

        public async Task updateTodoItemAsync(Guid id, TodoItem? todoItem)
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

            await todoRepository.updateTodoItemAsync(entity);
        }

        public async Task deleteTodoItemAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new MissingDataException("Id is required to delete a todo item.");
            }

            var task = await todoRepository.getTodoById(id);

            if(task == null)
            {
                throw new NotFoundException($"Todo item with id {id} was not found.");
            }

            await todoRepository.deleteTodoItemAsync(id);
        }

    }
}
