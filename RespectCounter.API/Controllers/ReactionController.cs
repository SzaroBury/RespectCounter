using System.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Mappers;
using RespectCounter.Application.Commands;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/reactions")]
public class ReactionController : ControllerBase
{
    private readonly ILogger<ReactionController> logger;
    private readonly ISender mediator;

    public ReactionController(ILogger<ReactionController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Queries

    #endregion

    #region Commands
    [HttpPost("/api/activity/{id}/reaction/{reaction}")]
    [Authorize]
    public async Task<IActionResult> ReactionToActivity(string id, int reaction)
    {
        logger.LogInformation($"{DateTime.Now}: ReactionToActivity(id: '{id}', reaction: {reaction})");
        var command = new AddReactionToActivityCommand(id, reaction, User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("/api/person/{id}/reaction/{reaction}")]
    [Authorize]
    public async Task<IActionResult> ReactionToPerson(string id, int reaction)
    {
        logger.LogInformation($"{DateTime.Now}: ReactionToPerson(id: '{id}', reaction: {reaction})");
        var command = new AddReactionToPersonCommand(id.ToGuid(), reaction, User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);    }

    [HttpPost("/api/comment/{id}/reaction/{reaction}")]
    [Authorize]
    public async Task<IActionResult> ReactToComment(string id, int reaction)
    {
        logger.LogInformation($"{DateTime.Now}: ReactToComment(id: '{id}', reaction: {reaction})");
        var command = new AddReactionToCommentCommand(id.ToGuid(), reaction, User.GetCurrentUserId());
        var result = await mediator.Send(command);
        return Ok(result);
    }  

    #endregion
}