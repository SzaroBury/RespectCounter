using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Mappers;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController: ControllerBase
{
    private readonly ILogger<CommentController> logger;
    private readonly ISender mediator;

    public CommentController(ILogger<CommentController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/activity/{id}/comments")]
    public async Task<IActionResult> GetCommentsForActivity(string id, [FromQuery] int level = 2, string? order = "")
    {
        logger.LogInformation($"{DateTime.Now}: GetCommentsForActivity(id: '{id}', level: {level})");
        var query = new GetCommentsForActivityQuery(
            id.ToGuid(),
            level,
            User.TryGetCurrentUserId(),
            order.ToCommentSortByEnum());
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("/api/person/{id}/comments")]
    public async Task<IActionResult> GetCommentsForPerson(string id, [FromQuery] int level = 2, string? order = "")
    {
        logger.LogInformation($"{DateTime.Now}: GetCommentsForPerson(id: '{id}', level: {level})");
        var query = new GetCommentsForPersonQuery(
            id.ToGuid(),
            level,
            User.TryGetCurrentUserId(),
            order.ToCommentSortByEnum());
        var result = await mediator.Send(query);
        return Ok(result);
    }

    #endregion

    #region Commands
    [Authorize]
    [HttpPost("{id}")]
    public async Task<IActionResult> CommentToComment(string id, [FromBody] string content)
    {
        logger.LogInformation($"{DateTime.Now}: CommentToComment(id: '{id}', content: '{content}')");
        var command = new AddCommentToParentCommentCommand(id.ToGuid(), content, User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/activity/{id}/comment")]
    [Authorize]
    public async Task<IActionResult> CommentActivity(string id, [FromBody] string content)
    {
        logger.LogInformation($"{DateTime.Now}: CommentActivity(id: '{id}', content: '{content}')");
        var command = new AddCommentToActivityCommand(id.ToGuid(), content, User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/person/{id}/comment")]
    [Authorize]
    public async Task<IActionResult> CommentPerson(string id, [FromBody] string content)
    {
        logger.LogInformation($"{DateTime.Now}: CommentPerson(id: '{id}', content: '{content}')");
        var command = new AddCommentToPersonCommand(id.ToGuid(), content, User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);;
    }

    [HttpPut("{id}")]
    [Authorize]
    public Task<IActionResult> UpdateComment(string id, [FromBody] string content)
    {
        logger.LogInformation($"{DateTime.Now}: UpdateComment(id: '{id}', content: '{content}')");
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> HideComment(string id)
    {
        logger.LogInformation($"{DateTime.Now}: HideComment(id: '{id}')");
        throw new NotImplementedException();
    }
    #endregion
}