using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Commands;
using RespectCounter.Application.Queries;
using RespectCounter.Domain.DTO;
using RespectCounter.Domain.Model;

namespace RespectCounter.API.Controllers;

//Get Persons by
//  Search: Tags, Firstname, Lastname, Birth date, Nationality, Respect, Verified/NotVerified
//      Sorting: Best Match, Most Respect, Least Respect, Trending (last 7d reactions and respect), Alfpha
//  Activity Id
//  ?IdentityUserUid
//  Pages/Autoload more?
//Get Person by Id

//Post a person
//Verify a person
//React
//Comment
//Add a tag
//Propose a change
//?Hide a person/Mark as invalid

//other todos: auto added tag based on nationality

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

    #region Querries
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

    #endregion

    #region Commands
    [HttpPost]
    public async Task<IActionResult> ProposePerson([FromBody] NewPerson newPerson)
    {
        var command = new AddPersonCommand(){ Person = newPerson };
        Person result = await mediator.Send(command);

        return Ok(result);
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