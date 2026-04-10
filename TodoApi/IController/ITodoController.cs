using Microsoft.AspNetCore.Mvc;
using TodoApi.DTO;
using TodoServices.Dto;

namespace TodoApi.IController
{
    public interface ITodoController
    {
        Task<ActionResult<List<TodoItemResponse>>> Get();
        Task<ActionResult<TodoItemResponse>> GetById(Guid id);
        Task<ActionResult> Create(CreateToDoItemRequestDto todoItem);
        Task<ActionResult> Update(Guid id, PatchToDoItemRequestDto todoItem);
        Task<ActionResult> Delete(Guid id);
    }
}
