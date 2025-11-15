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

    [HttpPut("{taskId}/title/{newTitle}")]
    public async Task<IActionResult> UpdateTaskTitle([FromRoute] Guid taskId, string newTitle)
    {
        var response = await Mediator.Send(new UpdateTaskTitleCommand(taskId, newTitle));

        return response.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPut("{taskId}/complete")]
    public async Task<IActionResult> CompleteTask([FromRoute] Guid taskId)
    {
        var response = await Mediator.Send(new CompleteTaskCommand(taskId));

        return response.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask([FromRoute] Guid taskId)
    {
        var response = await Mediator.Send(new DeleteTaskCommand(taskId));

        return response.Match(
            _ => NoContent(),
            Problem);
    }
}