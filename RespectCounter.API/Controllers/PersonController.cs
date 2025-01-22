using System.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.API.Models;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController: ControllerBase
{

    private readonly ILogger<PersonController> logger;
    private readonly ISender mediator;

    public PersonController(ILogger<PersonController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    #region Queries
    [HttpGet("/api/persons")]
    public async Task<IActionResult> GetVerifiedPersons([FromQuery] string search = "", [FromQuery] string order = "")
    {
        var query = new GetVerifiedPersonsQuery
        {
            Search = search,
            Order = order
        };
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("/api/persons/all")]
    public async Task<IActionResult> GetPersons([FromQuery] string search = "", [FromQuery] string order = "")
    {
        var query = new GetPersonsQuery(search, order);
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(string id)
    {
        var query = new GetPersonByIdQuery(id);

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
    public async Task<IActionResult> GetComments(string id, [FromQuery] int level = 2)
    {
        var query = new GetCommentsForPersonQuery(id, level);

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

    [HttpGet("/api/persons/names")]
    public async Task<IActionResult> GetSimplePersons([FromQuery] string search = "", [FromQuery] string order = "")
    {
        var query = new GetSimplePersonsQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}/tags")]
    public async Task<IActionResult> GetPersonTags(string id)
    {
        var query = new GetPersonTagsQuery(id);

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
    public async Task<IActionResult> ProposePerson([FromBody] ProposePersonModel newPerson)
    {
        var command = new AddPersonCommand(
            newPerson.FirstName, 
            newPerson.LastName,
            newPerson.NickName ?? "",
            newPerson.Description ?? "", 
            newPerson.Nationality, 
            newPerson.Birthday, 
            newPerson.DeathDate, 
            newPerson.Tags,
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
    public async Task<IActionResult> VerifyPerson(string id)
    {
        var command = new VerifyPersonCommand(){ Id = id };
        Person result = await mediator.Send(command);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id}/reaction/{reaction}")]
    public async Task<IActionResult> ReactionToPerson(string id, int reaction)
    {
        var command = new AddReactionToPersonCommand(){ PersonId = id, ReactionType = reaction, User = User};
        Person result = await mediator.Send(command);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id}/comment")]
    public async Task<IActionResult> CommentPerson(string id, [FromBody] string content)
    {
        var command = new AddCommentToPersonCommand(){ PersonId = id, Content = content, User = User};
        Person result = await mediator.Send(command);

        return Ok(result);;
    }

    [Authorize]
    [HttpPost("{id}/tag/{tag}")]
    public async Task<IActionResult> TagPerson(string id, string tag)
    {
        var command = new AddTagToPersonCommand(){ PersonId = id, TagName = tag, User = User};
        Person result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public Task<IActionResult> ProposeUpdatePerson(string id, [FromBody] Person person)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}/hide")]
    public Task<IActionResult> HidePerson(string id)
    {
        throw new NotImplementedException();
    }
    #endregion
}