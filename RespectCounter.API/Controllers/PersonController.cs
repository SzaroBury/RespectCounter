using MediatR;
using Microsoft.AspNetCore.Mvc;
using RespectCounter.Application.Queries;

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
//Propose a change
//Add a tag
//?Hide a person/Mark as invalid

//other todos: auto added tag based on nationality

[ApiController]
[Route("api/[controller]")]
public class PersonController: ControllerBase
{
    private readonly ILogger<PersonController> logger;
    private readonly ISender mediator;

    public PersonController(ILogger<PersonController> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet("api/persons")]
    public async Task<IActionResult> GetPersons()
    {
        var query = new GetPersonsQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(int id)
    {
        var query = new GetPersonByIdQuery()
        { 
            Id = id 
        };
        
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
}