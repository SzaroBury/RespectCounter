using MediatR;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Commands;

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
    
    #endregion

    #region Commands
    [HttpPost("{id}")]
    public async Task<IActionResult> CommentToComment(string id, [FromBody] string content)
    {
        var command = new AddCommentToParentCommentCommand(id, content, User);
        try
        {
            var result = await mediator.Send(command);
            
            return Ok(result);
        }
        catch(System.Security.SecurityException)
        {
            return Unauthorized();
        }
    }
  
    [HttpPost("{id}/reaction/{reaction}")]
    public async Task<IActionResult> ReactToComment(string id, int reaction)
    {
        var command = new AddReactionToCommentCommand(id, reaction, User);
        var result = await mediator.Send(command);

        return Ok(result);
    }      

    [HttpPut("{id}")]
    public Task<IActionResult> UpdateComment(string id, [FromBody] string content)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    public Task<IActionResult> HideComment(string id)
    {
        throw new NotImplementedException();
    }
    #endregion
}