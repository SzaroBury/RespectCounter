using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using MediatR;

using RespectCounter.Domain.Model;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;
using RespectCounter.API.Requests;
using RespectCounter.API.Mappers;

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
        logger.LogInformation($"{DateTime.Now}: GetActivities(search = '{search}', order = '{order}', tags = '{tags}')");
        var query = new GetActivitiesQuery(search, order.ToActivitySortByEnum(), tags, User.TryGetCurrentUserId());
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/activities")]
    public async Task<IActionResult> GetVerifiedActivities([FromQuery] string search = "", [FromQuery] string order = "", [FromQuery] string tags = "")
    {
        logger.LogInformation($"{DateTime.Now}: GetVerifiedActivities(search = '{search}', order = '{order}', tags = '{tags}')");
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "??";
        var query = new GetActivitiesQuery(search, order.ToActivitySortByEnum(), tags, User.TryGetCurrentUserId(), [ActivityStatus.Verified]);
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/person/{personId}/activities")]
    public async Task<IActionResult> GetActivitiesByPerson(string personId, string? type, string? order, bool? onlyVerified)
    {
        logger.LogInformation($"{DateTime.Now}: GetActivitiesByPerson(personId = '{personId}', type = '{type}', order = '{order}', onlyVerified = '{onlyVerified}')");
        var query = new GetActivitiesByPersonQuery(
            personId.ToGuid(),
            User.TryGetCurrentUserId(),
            order.ToActivitySortByEnum(),
            type.ToActivityTypeEnum(),
            onlyVerified.ToActivityStatusList());
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(string id)
    {
        logger.LogInformation($"{DateTime.Now}: GetActivity(id = '{id}')");

        var query = new GetActivityByIdQuery(id.ToGuid(), User.TryGetCurrentUserId());
        var result = await mediator.Send(query);
        return Ok(result);
    }
    #endregion
    
    #region Commands
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProposeActivity(ProposeActivityRequest newActivity)
    {
        logger.LogInformation($"{DateTime.Now}: ProposeActivity([ProposeActivityRequest])");
        var currentUserId = User.GetCurrentUserId(); 
        var command = newActivity.ToCommand(currentUserId);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/verify")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VerifyActivity(string id)
    {
        logger.LogInformation($"{DateTime.Now}: VerifyActivity(id: '{id}')");
        var command = new VerifyActivityCommand(id.ToGuid());
        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public Task<IActionResult> ProposeUpdateActivity(string id, [FromBody] Activity activity)
    {
        logger.LogInformation($"{DateTime.Now}: ProposeUpdateActivity(id: '{id}', [Activity])");
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    [Authorize(Roles = "Admin")]
    public Task<IActionResult> HideActivity(string id)
    {
        logger.LogInformation($"{DateTime.Now}: HideActivity(id: '{id}')");
        throw new NotImplementedException();
    }
    #endregion
    
}