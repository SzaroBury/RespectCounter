using MediatR;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Queries;

namespace RespectCounter.API.Controllers;

//Get Activities By
// Search: Tags, Description, Value, Respect, Happend, Verified/NotVerified
//  Sorting: Best Match, Most Respect, Least Respect, Trending (last 7d reactions and respect)
// PersonId
// ?IdentityUserUid
// Pages/Autoload more?
//Get Activity By Id

//Propose an Activity
//Verify an Activity
//React
//Comment
//?Propose a change
//?Hide a person
//?Add a tag

//other todos: auto added tag based on nationality

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
    public async Task<IActionResult> GetActivities([FromQuery] string search = "", [FromQuery] string order = "")
    {
        var query = new GetActivitesQuery(search, order);
        var result = await mediator.Send(query);

        return Ok(result);
    }
    #endregion
    
    
    
}