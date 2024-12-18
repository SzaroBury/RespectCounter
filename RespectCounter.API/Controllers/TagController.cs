using MediatR;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Queries;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

//Get Tags By
// PersonId
// ActivityId
// Level
// ?IdentityUserUid
// Pages/Autoload more?
//Get Tags By Id

//Post a tag
//?Edit a tag
//?Hide a comment/Mark as 

//other todos: auto added tag based on nationality

[ApiController]
[Route("api/tag")]
public class TagController: ControllerBase
{
    private readonly ILogger<TagController> logger;
    private readonly ISender mediator;

    public TagController(ILogger<TagController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/tags")]
    public async Task<IActionResult> GetTags()
    {
        try
        {
            var query = new GetTagsQuery(5);
            var result = await mediator.Send(query);

            return Ok(result);
        }
        catch(Exception e)
        {
            var errorResponse = new
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = $"An unexpected error occurred: '{e.Message}'."
            };
            return StatusCode(500, errorResponse);
        }
        
    }

    [HttpGet("/api/tags/recent")]
    public IActionResult GetRecentlyBrowsedTags()
    {
        try
        {
            // var query = new GetFavouriteTagsQuery();
            // var result = await mediator.Send(query);
            // throw new NotImplementedException();

            return Ok(new List<Tag>());
        }
        catch(Exception e)
        {
            var errorResponse = new
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = $"An unexpected error occurred: '{e.Message}'."
            };
            return StatusCode(500, errorResponse);
        }
    }

    [HttpGet("/api/tags/favourite")]
    public IActionResult GetFavouriteTags()
    {
        try
        {
            // var query = new GetFavouriteTagsQuery();
            // var result = await mediator.Send(query);
            // throw new NotImplementedException();

            return Ok(new List<Tag>());
        }
        catch(Exception e)
        {
            var errorResponse = new
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = $"An unexpected error occurred: '{e.Message}'."
            };
            return StatusCode(500, errorResponse);
        }
    }
    #endregion
}