using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PinoyCleanArch.Api.Controllers;
using PinoyTodo.Application.Tasks.Commands;
using PinoyTodo.Contracts;

namespace PinoyTodo.Api.Controllers;

[Route("[controller]")]
public class TasksController : BaseController
{
    public TasksController(ISender mediator, IMapper mapper)
        : base(mediator, mapper)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var id = await Mediator.Send(new CreateTaskCommand(request.Title));

        return Ok(id);
    }

    [HttpPost("{taskId}/complete")]
    public async Task<IActionResult> CompleteTask([FromRoute] Guid taskId)
    {
        await Mediator.Send(new CompleteTaskCommand(taskId));

        return NoContent();
    }
}