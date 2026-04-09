using Microsoft.AspNetCore.Mvc;
using TodoApi.DTO;
using TodoServices.Dto;
using TodoServices.Service;

namespace TodoApi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController(TodoService todoService) : Controller
    {
        // GET: TodoController
        [HttpGet]
        public async Task<ActionResult<List<TodoItemResponse>>> Get()
        {
            var result = await todoService.getAllTodosAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        // GET: TodoController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var result = await todoService.getTodoByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateToDoItemRequestDto todoItem)
        {
            var serviceTodoItem = new TodoItem
            {
                Title = todoItem.Title
            };

            await todoService.createTodoItemAsync(serviceTodoItem);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, [FromBody] CreateToDoItemRequestDto todoItem)
        {
            var serviceTodoItem = new TodoItem
            {
                Id = id,
                Title = todoItem.Title
            };

            await todoService.updateTodoItemAsync(id, serviceTodoItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await todoService.deleteTodoItemAsync(id);

            return NoContent();
        }

    }
}
