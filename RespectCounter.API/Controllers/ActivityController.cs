using MediatR;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Queries;
using RespectCounter.Application.Commands;
using RespectCounter.Domain.Model;
using RespectCounter.API.Models;
using System.Security;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/activity")]
public class ActivityController: ControllerBase
{
    private readonly ILogger<ActivityController> logger;
    private readonly ISender mediator;

    public ActivityController(ILogger<ActivityController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/activities/all")]
    public async Task<IActionResult> GetActivities([FromQuery] string search = "", [FromQuery] string order = "", [FromQuery] string tags = "")
    {
        Console.WriteLine($"GetActivities(search = '{search}', order = '{order}', tags = '{tags}')");
        var query = new GetActivitesQuery(search, order, tags);
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/activities")]
    public async Task<IActionResult> GetVerifiedActivities([FromQuery] string search = "", [FromQuery] string order = "", [FromQuery] string tags = "")
    {
        Console.WriteLine($"GetVerifiedActivities(search = '{search}', order = '{order}', tags = '{tags}')");
        var query = new GetActivitesQuery(search, order, tags, [ActivityStatus.Verified]);
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(string id)
    {
        var query = new GetActivityByIdQuery(id);

        try
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetCommentsForActivity(string id, [FromQuery] int level = 2)
    {
        var query = new GetCommentsForActivityQuery(id, level);

        try
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    #endregion
    
    #region Commands
    [HttpPost]
    public async Task<IActionResult> ProposeActivity([FromBody] ProposeActivityModel newActivity)
    {
        var command = new AddActivityCommand(
            newActivity.PersonId, 
            newActivity.Title, 
            newActivity.Description ?? "", 
            newActivity.Location ?? "", 
            newActivity.Happend ?? "", 
            newActivity.Source, 
            newActivity.Type, 
            newActivity.Tags,
            User
            );

        try
        {
            var result = await mediator.Send(command);

            return Ok(result);
        }   
        catch(ArgumentException e)
        {
            ModelState.AddModelError(e.ParamName ?? "??", e.Message);
            return ValidationProblem(ModelState);
        } 
        catch(SecurityException)
        {
            return Unauthorized();
        }
    }

    [HttpPut("{id}/verify")]
    public async Task<IActionResult> VerifyActivity(string id)
    {
        var command = new VerifyActivityCommand(id);
        Activity result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{id}/reaction/{reaction}")]
    public async Task<IActionResult> ReactionToActivity(string id, int reaction)
    {
        var command = new AddReactionToActivityCommand(id, reaction, User);
        Activity result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{id}/comment")]
    public async Task<IActionResult> CommentActivity(string id, [FromBody] string content)
    {
        var command = new AddCommentToActivityCommand(id, content, User);

        try
        {
            Activity result = await mediator.Send(command);
            
            return Ok(result);
        }
        catch(SecurityException)
        {
            return Unauthorized();
        }

    }

    [HttpPost("{id}/tag/{tag}")]
    public async Task<IActionResult> TagActivity(string id, string tag)
    {
        var command = new AddTagToActivityCommand(id, tag, User);
        Activity result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public Task<IActionResult> ProposeUpdateActivity(string id, [FromBody] Activity activity)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    public Task<IActionResult> HideActivity(string id)
    {
        throw new NotImplementedException();
    }
    #endregion
    
}