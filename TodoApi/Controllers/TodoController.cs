using Microsoft.AspNetCore.Mvc;
using TodoApi.DTO;
using TodoApi.IController;
using TodoServices.Dto;
using TodoServices.IService;

namespace TodoApi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController(ITodoService todoService) : Controller, ITodoController
    {
        // GET: TodoController
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<TodoItemResponse>>> Get()
        {
            var result = await todoService.getAllTodosAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        // GET: TodoController/Details/5
        public async Task<ActionResult<TodoItemResponse>> Details(Guid id)
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
        public async Task<ActionResult> Patch(Guid id, [FromBody] PatchToDoItemRequestDto todoItem)
        {
            var serviceTodoItem = new TodoItem
            {
                Id = id,
                Title = todoItem.Title,
                isCompleted = todoItem.isCompleted
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
